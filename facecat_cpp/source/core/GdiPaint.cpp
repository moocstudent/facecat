#include "..\\..\\stdafx.h"
#include "..\\..\\include\\core\\GdiPaint.h"

namespace FaceCat{
	void GdiPaint::affectScaleFactor(FCRect *rect){
		if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
            rect->left = (int)(rect->left * m_scaleFactorX);
            rect->top = (int)(rect->top * m_scaleFactorY);
            rect->right = (int)(rect->right * m_scaleFactorX);
            rect->bottom = (int)(rect->bottom * m_scaleFactorY);
        }
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////

	GdiPaint::GdiPaint(){
		m_hDC = 0;
		m_hRgn = 0;
		m_offsetX = 0;
		m_offsetY = 0;
		m_pRect.left = 0;
		m_pRect.top = 0;
		m_pRect.right = 0;
		m_pRect.bottom = 0;
		m_rotateAngle = 0;
		m_scaleFactorX = 0;
		m_scaleFactorY = 0;
		m_wRect.left = 0;
		m_wRect.top = 0;
		m_wRect.right = 0;
		m_wRect.bottom = 0;
		m_memBM = 0;
		m_wndHDC = 0;
	}

	GdiPaint::~GdiPaint(){
		if(m_hDC){
			DeleteDC(m_hDC);
			m_hDC = 0;
		}
		if (m_hRgn){
            DeleteObject(m_hRgn);
            m_hRgn = 0;
        }
		if(m_memBM){
			DeleteObject(m_memBM);
			m_memBM = 0;
		}
		clearCaches();
		m_wndHDC = 0;
	}

	void GdiPaint::addArc(const FCRect& rect, float startAngle, float sweepAngle){
	}
	
	void GdiPaint::addBezier(FCPoint *apt, int cpt){
	}
	
	void GdiPaint::addCurve(FCPoint *apt, int cpt){
	}
	
	void GdiPaint::addEllipse(const FCRect& rect){
	}
	
	void GdiPaint::addLine(int x1, int y1, int x2, int y2){
	}
	
	void GdiPaint::addRect(const FCRect& rect){
	}
	
	void GdiPaint::addPie(const FCRect& rect, float startAngle, float sweepAngle){
	}
	
	void GdiPaint::addText(const wchar_t *strText, FCFont *font, const FCRect& rect){
	}

	void GdiPaint::beginExport(const String& exportPath, const FCRect& rect){
	}

	void GdiPaint::beginPaint(HDC hDC, const FCRect& wRect, const FCRect& pRect){
		m_pRect = pRect;
		m_wRect = wRect;
		m_wndHDC = hDC;
		int width = m_wRect.right - m_wRect.left;
		int height = m_wRect.bottom - m_wRect.top;
		m_hDC = CreateCompatibleDC(hDC);
		m_memBM = CreateCompatibleBitmap(hDC, width,  height);
		m_resourcePath = L"";
		SelectObject(m_hDC, m_memBM);
		FCRect rc = {-1, -1, 1, 1};
		FCFont *font = new FCFont;
		drawText(L"", FCColor::argb(0, 0, 0), font, rc);
		delete font;
		font = 0;
	}

	void GdiPaint::beginPath(){
	}

	void GdiPaint::clearCaches(){
		std::map<String, Bitmap*>::iterator sIter = m_images.begin(); 
		for(;sIter != m_images.end(); ++sIter){
			delete sIter->second;
		}
		m_images.clear();
	}

	void GdiPaint::clipPath(){
	}

	void GdiPaint::closeFigure(){
	}
	
	void GdiPaint::closePath(){
	}

	void GdiPaint::drawArc(Long dwPenColor, float width, int style, const FCRect& rect, float startAngle, float sweepAngle){
	}
	
	void GdiPaint::drawBezier(Long dwPenColor, float width, int style, FCPoint *apt, int cpt){
	}
	
	void GdiPaint::drawCurve(Long dwPenColor, float width, int style, FCPoint *apt, int cpt){
	}

	void GdiPaint::drawEllipse(Long dwPenColor, float width, int style, const FCRect& rect){
		drawEllipse(dwPenColor, width, style, rect.left, rect.top, rect.right, rect.bottom);
	}

	void GdiPaint::drawEllipse(Long dwPenColor, float width, int style, int left, int top, int right, int bottom){
		if(dwPenColor == FCColor_None) return;
		dwPenColor = getPaintColor(dwPenColor);
		if(dwPenColor < 0 ) dwPenColor = -dwPenColor / 1000;
		HPEN hPen = ::CreatePen(style, (int)width, (int)dwPenColor); 
		HPEN hOldPen = (HPEN)::SelectObject(m_hDC, hPen);
		::SelectObject(m_hDC, ::GetStockObject(HOLLOW_BRUSH));
		FCRect newRect = {left + m_offsetX, top + m_offsetY, right + m_offsetX, bottom + m_offsetY};
		affectScaleFactor(&newRect);
		::Ellipse(m_hDC, newRect.left, newRect.top, newRect.right, newRect.bottom);
		::SelectObject(m_hDC, hOldPen);
		::DeleteObject(hPen); 
	}

	void GdiPaint::drawImage(const wchar_t *imagePath, const FCRect& rect){
		String imageKey = m_resourcePath + imagePath;
        Bitmap *drawImage = 0;
		int rw = rect.right - rect.left;
        if (rw < 1) rw = 1;
        int rh = rect.bottom - rect.top;
        if (rh < 1) rh = 1;
        Rect gdiPlusRect(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
        if (m_images.find(imageKey) != m_images.end()){
            drawImage = m_images[imageKey];
        }
        else{
            String attributes[5];
			attributes[0] = L"file";
			attributes[1] = L"corner";
			attributes[2] = L"source";
			attributes[3] = L"highcolor";
			attributes[4] = L"lowcolor";
			String values[5];
			values[0] = imagePath;
			values[1] = L"";
			values[2] = L"";
			values[3] = L"";
			values[4] = L"";
			String wPath = imagePath;
			if (wPath.find(L"=") >= 0){
				for (int i = 0; i < 5; i++){
					String attribute = attributes[i];
					int alength = (int)attribute.length() + 2;
					int pos = (int)wPath.find(attribute + L"=\'");
					if (pos >= 0){
						int rpos = (int)wPath.find(L"\'", pos + alength);
						values[i] = wPath.substr(pos + alength, rpos - pos - alength);
					}
				}
			}
			String file = values[0];
			string strFile = FCStr::wstringTostring(file);
			if(_access(strFile.c_str(), 0) == -1){
				if(m_resourcePath.length() > 0){
					if(m_resourcePath.find(L"\\") == (int)m_resourcePath.length() - 1){
						file = m_resourcePath + file;
					}
					else{
						file = m_resourcePath + L"\\" + file;
					}
				}
			}
			Bitmap *image = 0;
			if(values[2].length() > 0){
				int source[4];
				ArrayList<String> strs = FCStr::split(values[2], L",");
				int size = (int)strs.size();
				for (int i = 0; i < size; i++){
					int pos = FCStr::convertStrToInt(strs.get(i).c_str());
					source[i] = pos;
				}
				Rect gdiPlusSourceRect(source[0], source[1], source[2] - source[0], source[3] - source[1]);
				Bitmap *sourceImage = (Bitmap*)Bitmap::FromFile(file.c_str());
				if (sourceImage){
					image = new Bitmap(gdiPlusSourceRect.Width, gdiPlusSourceRect.Height);
					Graphics *sg = Graphics::FromImage(image);
					Rect srcRect(0, 0, gdiPlusSourceRect.Width, gdiPlusSourceRect.Height);
					sg->DrawImage(sourceImage, srcRect, gdiPlusSourceRect.GetLeft(), gdiPlusSourceRect.GetTop(), gdiPlusSourceRect.Width, gdiPlusSourceRect.Height, UnitPixel);
					delete sg;
					delete sourceImage;
				}
			}
			else{
				image = (Bitmap*)Bitmap::FromFile(file.c_str());
			}
			if(image){
				Gdiplus::ImageAttributes imageAttr;
				Long highColor = FCColor_None, lowColor = FCColor_None;
                if (values[3].length() != 0 && values[4].length() != 0){
					highColor = FCStr::convertStrToColor(values[3]);
					lowColor = FCStr::convertStrToColor(values[4]);
                }
                if (highColor != FCColor_None && lowColor != FCColor_None){
					int A = 0, R = 0, G = 0, B = 0;
					FCColor::toArgb(this, highColor, &A, &R, &G, &B);
                    Color gdiPlusHighColor((BYTE)A, (BYTE)R, (BYTE)G, (BYTE)B);
                    FCColor::toArgb(this, highColor, &A, &R, &G, &B);
                    Color gdiPlusLowColor((BYTE)A, (BYTE)R, (BYTE)G, (BYTE)B);
                    imageAttr.SetColorKey(gdiPlusLowColor, gdiPlusHighColor);
                }
				int iw = image->GetWidth();
				int ih = image->GetHeight();
				drawImage = new Bitmap(iw, ih);
				Graphics *g = Graphics::FromImage(drawImage);
				if (values[1].length() == 0){
					Rect drawImageRect(0, 0, iw, ih);
					g->DrawImage(image, drawImageRect, 0, 0, iw, ih, UnitPixel, &imageAttr);
				}
				else{
					int corners[4];
					ArrayList<String> strs = FCStr::split(values[1], L",");
					int size = (int)strs.size();
					for (int i = 0; i < size; i++){
						int corner = FCStr::convertStrToInt(strs.get(i).c_str());
						corners[i] = corner;
					}
					int left = 0;
					int top = 0;
					int right = iw;
					int bottom = ih;
					if (corners[0] > 0){
						Rect destRect(left, top, corners[0], ih);
						g->DrawImage(image, destRect, 0, 0, corners[0], ih, UnitPixel, &imageAttr);
					}
					if (corners[1] > 0){
						Rect destRect(left, top, iw, corners[1]);
						g->DrawImage(image, destRect, 0, 0, iw, corners[1], UnitPixel, &imageAttr);
					}
					if (corners[2] > 0){
						Rect destRect(right - corners[2], top, corners[2], ih);
						g->DrawImage(image, destRect, iw - corners[2], 0, corners[2], ih, UnitPixel, &imageAttr);
					}
					if (corners[3] > 0){
						Rect destRect(left, bottom - corners[3], iw, corners[3]);
						g->DrawImage(image, destRect, 0, ih - corners[3], iw, corners[3], UnitPixel, &imageAttr);
					}
					int cwdest = iw - corners[0] - corners[2];
					int chdest = ih - corners[1] - corners[3];
					int cwsrc = iw - corners[0] - corners[2];
					int chsrc = ih - corners[1] - corners[3];
					if (cwdest > 0 && chdest > 0 && cwsrc > 0 && chsrc > 0){
						Rect destRect(left + corners[0], top + corners[1], cwdest, chdest);
						g->DrawImage(image, destRect, corners[0], corners[1], cwsrc, chsrc, UnitPixel, &imageAttr);
					}
				}
				delete image;
				delete g;
				m_images[imageKey] = drawImage;
			}
        }
		if(drawImage){
			Color color(0, 0, 0);
			HBITMAP bitmap;
			drawImage->GetHBITMAP(color, &bitmap);
			HDC hdcsource = CreateCompatibleDC(m_hDC);
			SelectObject(hdcsource, bitmap);
			int left = rect.left + m_offsetX;
            int top = rect.top + m_offsetY;
            int width = rw;
            int height = rh;
            if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
                left = (int)(m_scaleFactorX * left);
                top = (int)(m_scaleFactorY * top);
                width = (int)(m_scaleFactorX * width);
                height = (int)(m_scaleFactorY * height);
            }
			StretchBlt(m_hDC, left, top, width, height, hdcsource, 0, 0, drawImage->GetWidth(), drawImage->GetHeight(), 13369376);
			DeleteObject(bitmap);
			DeleteObject(hdcsource);
		}
	}

	void GdiPaint::drawLine(Long dwPenColor, float width, int style, const FCPoint& x, const FCPoint& y){
		drawLine(dwPenColor, width, style, x.x, x.y, y.x, y.y);
	}

	void GdiPaint::drawLine(Long dwPenColor, float width, int style, int x1, int y1, int x2, int y2){
		if(dwPenColor == FCColor_None) return;
		dwPenColor = getPaintColor(dwPenColor);
		if(dwPenColor < 0 ) dwPenColor = -dwPenColor / 1000;
		HPEN hPen = ::CreatePen(style, (int)width, (int)dwPenColor); 
		HPEN hOldPen = (HPEN)::SelectObject(m_hDC, hPen);
		int lx1 = x1 + m_offsetX;
        int ly1 = y1 + m_offsetY;
        int lx2 = x2 + m_offsetX;
        int ly2 = y2 + m_offsetY;
        if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
            lx1 = (int)(m_scaleFactorX * lx1);
            ly1 = (int)(m_scaleFactorY * ly1);
            lx2 = (int)(m_scaleFactorX * lx2);
            ly2 = (int)(m_scaleFactorY * ly2);
        }
		::MoveToEx(m_hDC, lx1, ly1, 0); 
		::LineTo(m_hDC, lx2, ly2); 
		::SelectObject(m_hDC, hOldPen);
		::DeleteObject(hPen); 
	}

	void GdiPaint::drawPath(Long dwPenColor, float width, int style){
	}
	
	void GdiPaint::drawPie(Long dwPenColor, float width, int style, const FCRect& rect, float startAngle, float sweepAngle){
	}

	void GdiPaint::drawPolygon(Long dwPenColor, float width, int style, FCPoint *apt, int cpt){
		if(dwPenColor == FCColor_None) return;
		dwPenColor = getPaintColor(dwPenColor);
		if(dwPenColor < 0 ) dwPenColor = -dwPenColor / 1000;
		for(int i = 0; i< cpt; i++){
			int x = apt[i].x + m_offsetX;
			int y = apt[i].y + m_offsetY;
			if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
				x = (int)(m_scaleFactorX * x);
				y = (int)(m_scaleFactorY * y);
			}
			apt[i].x = x;
			apt[i].y = y;
		}
		HPEN hPen = ::CreatePen(style, (int)width, (int)dwPenColor); 
		HPEN hOldPen = (HPEN)::SelectObject(m_hDC, hPen);
		::Polygon(m_hDC,apt,cpt);
		::SelectObject(m_hDC, hOldPen);
		::DeleteObject(hPen); 
	}

	void GdiPaint::drawPolyline(Long dwPenColor, float width, int style, FCPoint *apt, int cpt){
		if(dwPenColor == FCColor_None) return;
		dwPenColor = getPaintColor(dwPenColor);
		if(dwPenColor < 0 ) dwPenColor = -dwPenColor / 1000;
		for(int i = 0; i< cpt; i++){
			int x = apt[i].x + m_offsetX;
			int y = apt[i].y + m_offsetY;
			if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
				x = (int)(m_scaleFactorX * x);
				y = (int)(m_scaleFactorY * y);
			}
			apt[i].x = x;
			apt[i].y = y;
		}
		HPEN hPen = ::CreatePen(style, (int)width, (int)dwPenColor); 
		HPEN hOldPen = (HPEN)::SelectObject(m_hDC, hPen);
		::Polyline(m_hDC, apt, cpt);
		::SelectObject(m_hDC, hOldPen);
		::DeleteObject(hPen); 
	}

	void GdiPaint::drawRect(Long dwPenColor, float width, int style, int left, int top, int right, int bottom){
		if(dwPenColor == FCColor_None) return;
		dwPenColor = getPaintColor(dwPenColor);
		if(dwPenColor < 0 ) dwPenColor = -dwPenColor / 1000;
		HPEN hPen = ::CreatePen(style, (int)width, (int)dwPenColor);
		HPEN hOldPen = (HPEN)::SelectObject(m_hDC, hPen);
		::SelectObject(m_hDC, ::GetStockObject(HOLLOW_BRUSH));
		FCRect newRect = {left + m_offsetX, top + m_offsetY, right + m_offsetX, bottom + m_offsetY};
		affectScaleFactor(&newRect);
		::Rectangle(m_hDC, newRect.left, newRect.top, newRect.right, newRect.bottom);
		::SelectObject(m_hDC, hOldPen);
		::DeleteObject(hPen);
	}

	void GdiPaint::drawRect(Long dwPenColor, float width, int style, const FCRect& rect){
		drawRect(dwPenColor, width, style, rect.left, rect.top, rect.right, rect.bottom);
	}

	void GdiPaint::drawRoundRect(Long dwPenColor, float width, int style, const FCRect& rect, int cornerRadius){
		if(dwPenColor == FCColor_None) return;
		dwPenColor = getPaintColor(dwPenColor);
		if(dwPenColor < 0 ) dwPenColor = -dwPenColor / 1000;
		HPEN hPen = ::CreatePen(style, (int)width, (int)dwPenColor);
		HPEN hOldPen = (HPEN)::SelectObject(m_hDC, hPen);
		::SelectObject(m_hDC, ::GetStockObject(HOLLOW_BRUSH));
		FCRect newRect = {rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY};
		affectScaleFactor(&newRect);
		if(cornerRadius != 0){
			::RoundRect(m_hDC, newRect.left, newRect.top, newRect.right, newRect.bottom, cornerRadius, cornerRadius);
		}
		else{
			::Rectangle(m_hDC, newRect.left, newRect.top, newRect.right, newRect.bottom);
		}
		::SelectObject(m_hDC, hOldPen);
		::DeleteObject(hPen);
	}

	void GdiPaint::drawText(const wchar_t *strText, Long dwPenColor, FCFont *font, const FCRect& rect){
		if(dwPenColor == FCColor_None) return;
		dwPenColor = getPaintColor(dwPenColor);
		if(dwPenColor < 0 ) dwPenColor = -dwPenColor / 1000;
		int fontSize = (int)font->m_fontSize;
		if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
			fontSize = (int)(fontSize * (m_scaleFactorX + m_scaleFactorY) / 2);
		}
		HFONT hFont = CreateFont
		(
			fontSize, 0,    
			0, 0,    
			font->m_bold? FW_BOLD: FW_REGULAR,    
			font->m_italic ? 1: 0,
			font->m_underline? 1: 0,
					0,
					GB2312_CHARSET,
					OUT_DEFAULT_PRECIS,
					CLIP_DEFAULT_PRECIS,
					DEFAULT_QUALITY,
					DEFAULT_PITCH | FF_SWISS,  
			font->m_fontFamily.c_str()    
		);
		FCRect newRect = {rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY};
		affectScaleFactor(&newRect);
		::SetBkMode(m_hDC, TRANSPARENT);
		::SetTextColor(m_hDC, (int)dwPenColor);
		HFONT hOldFont = (HFONT)::SelectObject(m_hDC, hFont);
		::DrawText(m_hDC, strText, -1, &newRect, 0 | DT_NOPREFIX);
		::SelectObject(m_hDC, hOldFont);
		::DeleteObject(hFont);
	}

	void GdiPaint::drawText(const wchar_t *strText, Long dwPenColor, FCFont *font, const FCRectF& rect){
		FCRect rc = {(int)rect.left, (int)rect.top, (int)rect.right, (int)rect.bottom};
		drawText(strText, dwPenColor, font, rc);
	}

	void GdiPaint::drawTextAutoEllipsis(const wchar_t *strText, Long dwPenColor, FCFont *font, const FCRect& rect){
		if(dwPenColor == FCColor_None) return;
		dwPenColor = getPaintColor(dwPenColor);
		if(dwPenColor < 0 ) dwPenColor = -dwPenColor / 1000;
		int fontSize = (int)font->m_fontSize;
		if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
			fontSize = (int)(fontSize * (m_scaleFactorX + m_scaleFactorY) / 2);
		}
		HFONT hFont = CreateFont
		(
			fontSize, 0,    
			0, 0,    
			font->m_bold? FW_BOLD: FW_REGULAR,    
			font->m_italic ? 1: 0,
			font->m_underline? 1: 0,
					0,
					GB2312_CHARSET,
					OUT_DEFAULT_PRECIS,
					CLIP_DEFAULT_PRECIS,
					DEFAULT_QUALITY,
					DEFAULT_PITCH | FF_SWISS,  
			font->m_fontFamily.c_str()    
		);
		FCRect newRect = {rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY};
		affectScaleFactor(&newRect);
		::SetBkMode(m_hDC, TRANSPARENT);
		::SetTextColor(m_hDC, (int)dwPenColor);
		HFONT hOldFont = (HFONT)::SelectObject(m_hDC, hFont);
		::DrawText(m_hDC, strText, -1, &newRect, 0 | DT_NOPREFIX | DT_WORD_ELLIPSIS);
		::SelectObject(m_hDC, hOldFont);
		::DeleteObject(hFont);
	}

	void GdiPaint::endExport(){
	}

	void GdiPaint::endPaint(){
		int left = m_pRect.left;
		int top = m_pRect.top;
		int width = m_pRect.right - m_pRect.left;
		int height = m_pRect.bottom - m_pRect.top;
		if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
			left = (int)(m_scaleFactorX * left);
			top = (int)(m_scaleFactorY * top);
			width = (int)(m_scaleFactorX * width);
			height = (int)(m_scaleFactorY * height);
		}
		BitBlt(m_wndHDC, left, top, width, height, m_hDC, left, top, SRCCOPY);
		if(m_hDC){
			DeleteDC(m_hDC);
			m_hDC = 0;
		}
		if(m_hRgn){
			DeleteObject(m_hRgn);
			m_hRgn = 0;
		}
		if(m_memBM){
			DeleteObject(m_memBM);
			m_memBM = 0;
		}
		m_offsetX = 0;
		m_offsetY = 0;
		m_resourcePath = L"";
		m_wndHDC = 0;
	}

	void GdiPaint::excludeClipPath(){
	}

	void GdiPaint::fillEllipse(Long dwPenColor, const FCRect& rect){
		if(dwPenColor == FCColor_None) return;
		dwPenColor = getPaintColor(dwPenColor);
		if(dwPenColor < 0 ) dwPenColor = -dwPenColor / 1000;
		HBRUSH brush = ::CreateSolidBrush((int)dwPenColor);
		::SelectObject(m_hDC, brush);
		FCRect newRect = {rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY};
		affectScaleFactor(&newRect);
		::Ellipse(m_hDC, newRect.left, newRect.top, newRect.right, newRect.bottom);
		::DeleteObject(brush);
	}

	void GdiPaint::fillGradientEllipse(Long dwFirst, Long dwSecond, const FCRect& rect, int angle){
		fillEllipse(dwFirst, rect);
	}

	void GdiPaint::fillGradientPath(Long dwFirst, Long dwSecond, const FCRect& rect, int angle){
	}

	void GdiPaint::fillGradientPolygon(Long dwFirst, Long dwSecond, FCPoint *apt, int cpt, int angle){
		fillPolygon(dwFirst, apt, cpt);
	}

	void GdiPaint::fillGradientRect(Long dwFirst, Long dwSecond, const FCRect& rect, int cornerRadius, int angle){
		if(cornerRadius != 0){
			fillRoundRect(dwFirst, rect, cornerRadius);
		}
		else{
			fillRect(dwFirst, rect);
		}
	}

	void GdiPaint::fillPath(Long dwPenColor){
	}
	
	void GdiPaint::fillPie(Long dwPenColor, const FCRect& rect, float startAngle, float sweepAngle){
	}

	void GdiPaint::fillPolygon(Long dwPenColor, FCPoint *apt, int cpt){
		if(dwPenColor == FCColor_None) return;
		dwPenColor = getPaintColor(dwPenColor);
		if(dwPenColor < 0 ) dwPenColor = -dwPenColor / 1000;
		for(int i = 0; i< cpt; i++){
			int x = apt[i].x + m_offsetX;
			int y = apt[i].y + m_offsetY;
			if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
				x = (int)(m_scaleFactorX * x);
				y = (int)(m_scaleFactorY * y);
			}
			apt[i].x = x;
			apt[i].y = y;
		}
		HBRUSH brush = ::CreateSolidBrush((int)dwPenColor);
		::SelectObject(m_hDC, brush);
		::Polygon(m_hDC, apt, cpt);
		::DeleteObject(brush);
	}

	void GdiPaint::fillRect(Long dwPenColor, const FCRect& rect){
		if(dwPenColor == FCColor_None) return;
		dwPenColor = getPaintColor(dwPenColor);
		if(dwPenColor < 0 ) dwPenColor = -dwPenColor / 1000;
		FCRect newRect = {rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY};
		affectScaleFactor(&newRect);
		HBRUSH brush = ::CreateSolidBrush((int)dwPenColor);
		::SelectObject(m_hDC, brush);
		::FillRect(m_hDC, &newRect, brush);
		::DeleteObject(brush);
	}

	void GdiPaint::fillRect(Long dwPenColor, int left, int top, int right, int bottom){
		FCRect newRect = {left, top, right, bottom};
		fillRect(dwPenColor, newRect);
	}

	void GdiPaint::fillRoundRect(Long dwPenColor, const FCRect& rect, int cornerRadius){
		if(dwPenColor == FCColor_None) return;
		dwPenColor = getPaintColor(dwPenColor);
		if(dwPenColor < 0 ) dwPenColor = -dwPenColor / 1000;
		FCRect newRect = {rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY};
		affectScaleFactor(&newRect);
		HBRUSH brush = ::CreateSolidBrush((int)dwPenColor);
		::SelectObject(m_hDC, brush);
		if(cornerRadius != 0){
			::RoundRect(m_hDC, newRect.left, newRect.top, newRect.right, newRect.bottom, cornerRadius, cornerRadius);
		}
		else{
			::FillRect(m_hDC, &newRect, brush);
		}
		::DeleteObject(brush);
	}

	Long GdiPaint::getColor(Long dwPenColor){
		if (dwPenColor < FCColor_None){
            if (dwPenColor == FCColor_Back){
                dwPenColor = 16777215;
            }
            else if (dwPenColor == FCColor_Border){
                dwPenColor = 3289650;
            }
            else if (dwPenColor == FCColor_Text){
                dwPenColor = 0;
            }
            else if (dwPenColor == FCColor_DisabledBack){
                dwPenColor = 13158600;
            }
            else if (dwPenColor == FCColor_DisabledText){
                dwPenColor = 3289650;
            }
			else if(dwPenColor == FCColor_Hovered){
				dwPenColor = 13158600;
			}
			else if(dwPenColor == FCColor_Pushed){
				dwPenColor = 9868950;
			}
        }
		return dwPenColor;
	}

	Long GdiPaint::getPaintColor(Long dwPenColor){
		return getColor(dwPenColor);
	}

	FCPoint GdiPaint::getOffset(){
		FCPoint offset = {m_offsetX, m_offsetY};
		return offset;
	}

	FCPoint GdiPaint::rotate(const FCPoint& op, const FCPoint& mp, int angle){
		FCPoint pt = {0};
		return pt;
	}

	void GdiPaint::setClip(const FCRect& rect){
		if(m_hRgn){
			DeleteObject(m_hRgn);
		}
		FCRect newRect = {rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY};
		if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
            newRect.left = (int)floor(newRect.left * m_scaleFactorX);
            newRect.top = (int)floor(newRect.top * m_scaleFactorY);
            newRect.right = (int)ceil(newRect.right * m_scaleFactorX);
            newRect.bottom = (int)ceil(newRect.bottom * m_scaleFactorY);
        }
		m_hRgn = ::CreateRectRgnIndirect(&newRect);
		::SelectClipRgn(m_hDC, m_hRgn);
	}

	void GdiPaint::setLineCap(int startLineCap, int endLineCap){
	}

	void GdiPaint::setOffset(const FCPoint& offset){
		m_offsetX = offset.x;
		m_offsetY = offset.y;
	}

	void GdiPaint::setOpacity(float opacity){
	}

	void GdiPaint::setResourcePath(const String& resourcePath){
		m_resourcePath = resourcePath;
	}

	void GdiPaint::setRotateAngle(int rotateAngle){
		m_rotateAngle = rotateAngle;
	}

	void GdiPaint::setScaleFactor(double scaleFactorX, double scaleFactorY){
		m_scaleFactorX = scaleFactorX;
		m_scaleFactorY = scaleFactorY;
	}

	void GdiPaint::setSmoothMode(int smoothMode){
	}

	void GdiPaint::setTextQuality(int textQuality){
	}

	bool GdiPaint::supportTransparent(){
		return false;
	}

	FCSize GdiPaint::textSize(const wchar_t *strText, FCFont *font){
		FCSize size = { 0 };
		HFONT hFont = CreateFont
		(
			(int)font->m_fontSize, 0,    
			0, 0,    
			font->m_bold? FW_BOLD: FW_REGULAR,    
			font->m_italic ? 1: 0,
			font->m_underline? 1: 0,
					0,
					GB2312_CHARSET,
					OUT_DEFAULT_PRECIS,
					CLIP_DEFAULT_PRECIS,
					DEFAULT_QUALITY,
					DEFAULT_PITCH | FF_SWISS, 
			font->m_fontFamily.c_str()    
		);
		HFONT hOldFont = (HFONT)::SelectObject(m_hDC, hFont);
		::GetTextExtentPoint32(m_hDC, strText, (int)_tcslen(strText), &size);
		::SelectObject(m_hDC, hOldFont);
		::DeleteObject(hFont);
		return size;
	}

	FCSizeF GdiPaint::textSizeF(const wchar_t *strText, FCFont *font){
		FCSize size = textSize(strText, font);
		FCSizeF fSize = {(float)size.cx, (float)size.cy};
		return fSize;
	}
}