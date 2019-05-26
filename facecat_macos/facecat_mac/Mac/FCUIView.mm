#import <Foundation/Foundation.h>

#import "FCUIView.h"
#include "IOSHost.h"
#include "stdafx.h"
#include "UIXmlEx.h"
#import "SafeCompile.h"

@interface FCUIView(){
    ContextPaint *m_contextPaint;
    FCTextBox *m_editingTextBox;
    IOSHost *m_host;
    bool m_leftIsDown;
    FCNative *m_native;
    FCPaint *m_paint;
    bool m_rightIsDown;
    NSTextField *m_textBox;
    FCUIXml *m_xml;
}
@end

@implementation FCUIView

-(void)drawRect:(CGRect)rect{
    [self onPaint:rect];
}

-(FCNative*)getNative{
    return m_native;
}

-(FCPaint*)getPaint{
    return m_paint;
}

-(void)setXml:(FCUIXml*)xml{
    m_xml = xml;
}

-(void)onLoad{
    if(!m_native){
        m_native = new FCNative;
        m_host = new IOSHost;
        m_contextPaint = new ContextPaint;
        m_paint = m_contextPaint;
        m_native->setHost(m_host);
        m_native->setPaint(m_contextPaint);
        m_host->setNative(m_native);
        m_host->setView(self);
        FCSize size = IOSHost::getSize(self.frame.size);
        m_native->setDisplaySize(size);
        NSTrackingArea *area = [[NSTrackingArea alloc] initWithRect:self.bounds
                                                            options:NSTrackingMouseMoved |NSTrackingMouseEnteredAndExited|NSTrackingActiveInKeyWindow
                                                              owner:self
                                                           userInfo:nil];
        [self addTrackingArea:area];
    }
}

-(void)onPaint:(CGRect)rect{
    if(m_native){
        FCSize size = IOSHost::getSize(self.frame.size);
        m_native->setDisplaySize(size);
        m_native->update();
        int width = rect.size.width, height = rect.size.height;
        if(m_host){
            FCRect pRect = {(int)rect.origin.x, (int)rect.origin.y, (int)(rect.origin.x + rect.size.width), (int)(rect.origin.y + rect.size.height)};
            pRect.top = size.cy - pRect.top - (pRect.bottom - pRect.top);
            pRect.bottom = pRect.top + height;
            double scaleFactorX = 1, scaleFactorY = 1;
            FCSize clientSize = IOSHost::getSize(rect.size);
            if (m_native->allowScaleSize()){
                if (clientSize.cx > 0 && clientSize.cy > 0){
                    FCSize scaleSize = m_native->getScaleSize();
                    scaleFactorX = (double)(clientSize.cx) / scaleSize.cx;
                    scaleFactorY = (double)(clientSize.cy) / scaleSize.cy;
                }
            }
            FCRect newRect = {0};
            if(scaleFactorX > 0 && scaleFactorY > 0){
                newRect.left = (int)(pRect.left / scaleFactorX);
                newRect.top = (int)(pRect.top / scaleFactorY);
                newRect.right = (int)(pRect.right / scaleFactorX);
                newRect.bottom = (int)(pRect.bottom / scaleFactorY);
            }
            m_host->onPaint(newRect);
        }
    }
}

-(void)showTextBox:(FCTextBox*)textBox{
    float scaleFactorX = 1, scaleFactorY = 1;
    if(m_native->allowScaleSize()){
        FCSize scaleSize = m_native->getScaleSize();
        IOSHost *host = dynamic_cast<IOSHost*>(m_native->getHost());
        FCSize size = host->getClientSize();
        if (size.cx > 0 & size.cy > 0){
            scaleFactorX = (float)scaleSize.cx / size.cx;
            scaleFactorY = (float)scaleSize.cy / size.cy;
        }
    }
    int subWidth = 0;
    ArrayList<FCView*> subControls = textBox->m_controls;
    int subControlsSize = (int)subControls.size();
    for(int i = 0; i < subControlsSize; i++){
        FCView *control = subControls.get(i);
        if(control->isVisible()){
            subWidth = control->getWidth();
        }
    }
    int x = (int)(m_native->clientX(textBox) / scaleFactorX) + 1;
    int y = (int)(m_native->clientY(textBox) / scaleFactorY) + 1;
    int cx = (int)((textBox->getWidth() - subWidth) / scaleFactorX) - 2;
    int cy = (int)(textBox->getHeight() / scaleFactorY) - 2;
    CGRect rect = CGRectMake(x, self.frame.size.height - y - cy, cx, cy);
    if(!m_textBox){
        m_textBox = [[NSTextField alloc] initWithFrame:rect];
        m_textBox.focusRingType = NSFocusRingTypeNone;
        [m_textBox setHighlighted:NO];
        m_textBox.editable = YES;
        m_textBox.bordered = NO;
        m_textBox.wantsLayer = YES;
        m_textBox.layer.borderColor = [NSColor whiteColor].CGColor;
        m_textBox.layer.borderWidth = 0.0f;
        [[m_textBox cell] setHighlighted:NO];
        [[m_textBox cell] setBezeled:NO];
        [[m_textBox cell] setWraps:NO];
        [[m_textBox cell] setBezeled:NO];
        [[m_textBox cell] setBordered:NO];
    }
    if(![m_textBox superview]){
        [self addSubview:m_textBox];
    }
    [SafeCompile setTextFieldDelegate:m_textBox withView:self];
    m_textBox.frame = rect;
    m_textBox.stringValue = IOSHost::getNSString(textBox->getText().c_str());
    [m_textBox becomeFirstResponder];
    [self setNeedsDisplay:YES];
}

-(void)hideTextBox:(FCTextBox*)textBox{
    if(m_textBox){
        if([m_textBox superview]){
            [m_textBox setHidden:YES];
            if(textBox){
                string text = [[m_textBox stringValue] UTF8String];
                String wText = FCStr::stringTowstring(text);
                textBox->setText(wText);
                int textLength = (int)wText.length();
                if(textLength > 0){
                    m_editingTextBox->setSelectionStart(textLength + 1);
                }
                FCView *parent = m_editingTextBox->getParent();
                if(parent){
                    FCGrid *grid = dynamic_cast<FCGrid*>(parent);
                    if(grid){
                        if(m_editingTextBox == grid->getEditTextBox()){
                            m_editingTextBox->setFocused(false);
                        }
                    }
                }
                m_editingTextBox = 0;
            }
            [m_textBox removeFromSuperview];
            m_textBox = nil;
        }
    }
}

-(void)mouseDown:(NSEvent *)event{
    m_leftIsDown = true;
    if(m_native){
        if(m_editingTextBox){
            [self hideTextBox:m_editingTextBox];
        }
        FCView *oldFocusedControl = m_native->getFocusedControl();
        NSPoint nmp = [self convertPoint:[event locationInWindow] fromView:nil];
        FCPoint mp = IOSHost::getPoint(nmp);
        mp.y = self.frame.size.height - mp.y;
        m_host->setTouchPoint(mp);
        int clicks = (int)event.clickCount;
        FCTouchInfo touchInfo;
        touchInfo.m_firstTouch = true;
        touchInfo.m_clicks = clicks;
        m_native->onMouseDown(touchInfo);
        FCView *newFocusedControl = m_native->getFocusedControl();
        if(clicks == 1){
            FCTextBox *oldTextBox = 0;
            if(oldFocusedControl){
                oldTextBox = dynamic_cast<FCTextBox*>(oldFocusedControl);
            }
            bool showTextBox = false;
            if(newFocusedControl){
                FCTextBox *textBox = dynamic_cast<FCTextBox*>(newFocusedControl);
                if(textBox && textBox->isEnabled() && !textBox->isReadOnly()){
                    m_editingTextBox = textBox;
                    m_editingTextBox->setFocused(false);
                    [self showTextBox:m_editingTextBox];
                    showTextBox = true;
                }
            }
            if(!showTextBox){
                [self hideTextBox:oldTextBox];
            }
        }
    }
}

-(void)rightMouseDown:(NSEvent *)event{
    m_rightIsDown = true;
    if(m_native){
        NSPoint nmp = [self convertPoint:[event locationInWindow] fromView:nil];
        FCPoint mp = IOSHost::getPoint(nmp);
        mp.y = self.frame.size.height - mp.y;
        m_host->setTouchPoint(mp);
        FCTouchInfo touchInfo;
        touchInfo.m_secondTouch = true;
        touchInfo.m_clicks = (int)event.clickCount;
        m_native->onMouseDown(touchInfo);
    }
}

-(void)mouseMoved:(NSEvent *)event{
    if(m_native){
        NSPoint nmp = [self convertPoint:[event locationInWindow] fromView:nil];
        FCPoint mp = IOSHost::getPoint(nmp);
        mp.y = self.frame.size.height - mp.y;
        m_host->setTouchPoint(mp);
        if(m_leftIsDown){
            FCTouchInfo touchInfo;
            touchInfo.m_firstTouch = true;
            touchInfo.m_clicks = (int)event.clickCount;
            m_native->onMouseMove(touchInfo);
        }
        else if(m_rightIsDown){
            FCTouchInfo touchInfo;
            touchInfo.m_secondTouch = true;
            touchInfo.m_clicks = (int)event.clickCount;
            m_native->onMouseMove(touchInfo);
        }
        else{
            FCTouchInfo touchInfo;
            m_native->onMouseMove(touchInfo);
        }
    }
}

-(void)mouseUp:(NSEvent *)event{
    m_leftIsDown = false;
    if(m_native){
        NSPoint nmp = [self convertPoint:[event locationInWindow] fromView:nil];
        FCPoint mp = IOSHost::getPoint(nmp);
        mp.y = self.frame.size.height - mp.y;
        m_host->setTouchPoint(mp);
        FCTouchInfo touchInfo;
        touchInfo.m_firstTouch = true;
        touchInfo.m_clicks = (int)event.clickCount;
        m_native->onMouseUp(touchInfo);
        FCView *focusedControl = m_native->getFocusedControl();
        FCTextBox *textBox = dynamic_cast<FCTextBox*>(focusedControl);
        if(textBox){
            FCView *parent = textBox->getParent();
            if(parent){
                FCGrid *grid = dynamic_cast<FCGrid*>(parent);
                if(grid){
                    if(textBox == grid->getEditTextBox()){
                        ArrayList<FCGridRow*> selectedRows = grid->getSelectedRows();
                        int selectedRowsSize = (int)selectedRows.size();
                        if(selectedRowsSize > 0){
                            FCGridRow *selectedRow = selectedRows.get(0);
                            FCRect bounds = selectedRow->getBounds();
                            FCHScrollBar *hScrollBar = grid->getHScrollBar();
                            FCVScrollBar *vScrollBar = grid->getVScrollBar();
                            int offsetX = 0, offsetY = 0;
                            if(hScrollBar && hScrollBar->isVisible()){
                                offsetX = hScrollBar->getPos();
                            }
                            if(vScrollBar && vScrollBar->isVisible()){
                                offsetY = vScrollBar->getPos();
                            }
                            ArrayList<FCGridColumn*> columns = grid->m_columns;
                            ArrayList<FCGridCell*> selectedCells = grid->getSelectedCells();
                            int columnsSize = (int)columns.size();
                            int left = 0, right = 0;
                            for(int i = 0; i < columnsSize; i++){
                                FCGridColumn *column = columns.get(i);
                                FCGridCell *cell = selectedRow->getCell(column->getName());
                                right += column->getWidth();
                                if(cell == selectedCells.get(0)){
                                    break;
                                }
                                left += column->getWidth();
                            }
                            bounds.left = left;
                            bounds.right = right;
                            bounds.left -=  offsetX;
                            bounds.right -= offsetX;
                            bounds.top -= offsetY;
                            bounds.bottom -= offsetY;
                            m_editingTextBox = textBox;
                            m_editingTextBox->setBounds(bounds);
                            [self showTextBox:m_editingTextBox];
                        }
                    }
                }
            }
        }
    }
}

-(void)mouseDragged:(NSEvent *)event{
    if(m_native){
        NSPoint nmp = [self convertPoint:[event locationInWindow] fromView:nil];
        FCPoint mp = IOSHost::getPoint(nmp);
        mp.y = self.frame.size.height - mp.y;
        m_host->setTouchPoint(mp);
        if(m_leftIsDown){
            FCTouchInfo touchInfo;
            touchInfo.m_firstTouch = true;
            touchInfo.m_clicks = (int)event.clickCount;
            m_native->onMouseMove(touchInfo);
        }
        else if(m_rightIsDown){
            FCTouchInfo touchInfo;
            touchInfo.m_secondTouch = true;
            touchInfo.m_clicks = (int)event.clickCount;
            m_native->onMouseMove(touchInfo);
        }
        else{
            FCTouchInfo touchInfo;
            m_native->onMouseMove(touchInfo);
        }
    }
}

-(void)rightMouseDragged:(NSEvent *)event{
    if(m_native){
        NSPoint nmp = [self convertPoint:[event locationInWindow] fromView:nil];
        FCPoint mp = IOSHost::getPoint(nmp);
        mp.y = self.frame.size.height - mp.y;
        m_host->setTouchPoint(mp);
        if(m_leftIsDown){
            FCTouchInfo touchInfo;
            touchInfo.m_firstTouch = true;
            touchInfo.m_clicks = (int)event.clickCount;
            m_native->onMouseMove(touchInfo);
        }
        else if(m_rightIsDown){
            FCTouchInfo touchInfo;
            touchInfo.m_secondTouch = true;
            touchInfo.m_clicks = (int)event.clickCount;
            m_native->onMouseMove(touchInfo);
        }
        else{
            FCTouchInfo touchInfo;
            m_native->onMouseMove(touchInfo);
        }
    }
}

-(void)rightMouseUp:(NSEvent *)event{
    m_rightIsDown = false;
    if(m_native){
        NSPoint nmp = [self convertPoint:[event locationInWindow] fromView:nil];
        FCPoint mp = IOSHost::getPoint(nmp);
        mp.y = self.frame.size.height - mp.y;
        m_host->setTouchPoint(mp);
        FCTouchInfo touchInfo;
        touchInfo.m_secondTouch = true;
        touchInfo.m_clicks = 1;
        m_native->onMouseUp(touchInfo);
    }
}

- (void)scrollWheel:(NSEvent *)event{
    if(m_native){
        NSPoint nmp = [self convertPoint:[event locationInWindow] fromView:nil];
        FCPoint mp = IOSHost::getPoint(nmp);
        mp.y = self.frame.size.height - mp.y;
        m_host->setTouchPoint(mp);
        CGFloat deltaX = [event scrollingDeltaX];
        CGFloat deltaY = [event scrollingDeltaY];
        if(deltaY > 0){
            FCTouchInfo touchInfo;
            touchInfo.m_firstTouch = true;
            touchInfo.m_delta = 1;
            m_native->onMouseWheel(touchInfo);
        }
        else if(deltaY < 0){
            FCTouchInfo touchInfo;
            touchInfo.m_firstTouch = true;
            touchInfo.m_delta = -1;
            m_native->onMouseWheel(touchInfo);
        }
    }
}

@end
