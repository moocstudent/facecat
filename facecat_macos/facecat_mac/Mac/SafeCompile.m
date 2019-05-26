#import <Foundation/Foundation.h>
#import "SafeCompile.h"

@implementation SafeCompile

+(void)setTextFieldDelegate:(NSTextField*)textField withView:(NSView*)nsView{
    textField.delegate = nsView;
}


@end
