#include "..\\..\\stdafx.h"
#include "..\\..\\include\\core\\GdiPlusPaint.h"

namespace FaceCat{
	void GdiPlusPaint::affectScaleFactor(Rect *gdiplusRect){
		if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
            gdiplusRect->X = (int)(gdiplusRect->X * m_scaleFactorX);
            gdiplusRect->Y = (int)(gdiplusRect->Y * m_scaleFactorY);
            gdiplusRect->Width = (int)(gdiplusRect->Width * m_scaleFactorX);
            gdiplusRect->Height = (int)(gdiplusRect->Height * m_scaleFactorY);
        }
	}

	void GdiPlusPaint::affectScaleFactor(RectF *gdiplusRect){
		if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
            gdiplusRect->X = (float)(gdiplusRect->X * m_scaleFactorX);
            gdiplusRect->Y = (float)(gdiplusRect->Y * m_scaleFactorY);
            gdiplusRect->Width = (float)(gdiplusRect->Width * m_scaleFactorX);
            gdiplusRect->Height = (float)(gdiplusRect->Height * m_scaleFactorY);
        }
	}

	SolidBrush* GdiPlusPaint::getBrush(Long dwPenColor){
	    Color gdiColor = getGdiPlusColor(dwPenColor);
        if (!m_brush){
            m_brush = new SolidBrush(gdiColor);
            if (m_opacity == 1){
                m_brushColor = dwPenColor;
            }
            else{
                m_brushColor = FCColor_None;
            }
        }
        else{
            if (m_brushColor == FCColor_None || m_brushColor != dwPenColor){
                m_brush->SetColor(gdiColor);
                m_brushColor = dwPenColor;
            }
			if(m_opacity != 1){
				m_brushColor = FCColor_None;
			}
        }
        return m_brush;
	}

	Color GdiPlusPaint::getGdiPlusColor(Long dwPenColor){
		dwPenColor = getPaintColor(dwPenColor);
		int a = 255;
		if(dwPenColor < 0){
			dwPenColor = -dwPenColor;
			if(dwPenColor < 1){
				a = 255;
			}
			else{
				a = (int)(dwPenColor - dwPenColor / 1000 * 1000);
			}
			dwPenColor /= 1000;
		}
		int r = (int)(dwPenColor & 0xff);
		int g = (int)((dwPenColor >> 8) & 0xff);
		int b = (int)((dwPenColor >> 0x10) & 0xff);
		Color gdiColor(a, r, g, b);
		if(m_opacity < 1){
			Color opacityColor((BYTE)(a * m_opacity), r, g, b);
			return opacityColor;
		}
		return gdiColor;
	}

	int GdiPlusPaint::getEncoderClsid(const WCHAR *format, CLSID *pClsid){
		UINT  num = 0;          
		UINT  size = 0;         
		ImageCodecInfo *pImageCodecInfo = 0;
		GetImageEncodersSize(&num, &size);
		if(size == 0)
			return -1;  
		pImageCodecInfo = (ImageCodecInfo*)(malloc(size));
		if(pImageCodecInfo == 0)
			return -1; 
		GetImageEncoders(num, size, pImageCodecInfo);
		for(UINT j = 0; j < num; ++j){
			if( wcscmp(pImageCodecInfo[j].MimeType, format) == 0 ){
				*pClsid = pImageCodecInfo[j].Clsid;
				free(pImageCodecInfo);
				return j;  
			}    
		}
		free(pImageCodecInfo);
		return -1;  
	}

	Gdiplus::Font* GdiPlusPaint::getFont(FCFont *font){
		if(font->m_strikeout){
			if (font->m_bold && font->m_underline && font->m_italic){
				return new Gdiplus::Font(font->m_fontFamily.c_str(), (REAL)font->m_fontSize, FontStyleBold | FontStyleUnderline | FontStyleItalic | FontStyleStrikeout, UnitPixel);
			}
			else if (font->m_bold && font->m_underline){
				return new Gdiplus::Font(font->m_fontFamily.c_str(), (REAL)font->m_fontSize, FontStyleBold | FontStyleUnderline | FontStyleStrikeout, UnitPixel);
			}
			else if (font->m_bold && font->m_italic){
				return new Gdiplus::Font(font->m_fontFamily.c_str(), (REAL)font->m_fontSize, FontStyleBold | FontStyleItalic | FontStyleStrikeout, UnitPixel);
			}
			else if (font->m_underline && font->m_italic){
				return new Gdiplus::Font(font->m_fontFamily.c_str(), (REAL)font->m_fontSize, FontStyleUnderline | FontStyleItalic | FontStyleStrikeout, UnitPixel);
			}
			else if (font->m_bold){
				return new Gdiplus::Font(font->m_fontFamily.c_str(), (REAL)font->m_fontSize, FontStyleBold | FontStyleStrikeout, UnitPixel);
			}
			else if(font->m_underline){
				return new Gdiplus::Font(font->m_fontFamily.c_str(), (REAL)font->m_fontSize, FontStyleUnderline | FontStyleStrikeout, UnitPixel);
			}
			else if (font->m_italic){
				return new Gdiplus::Font(font->m_fontFamily.c_str(), (REAL)font->m_fontSize, FontStyleItalic| FontStyleStrikeout, UnitPixel);
			}
			else{
				return new Gdiplus::Font(font->m_fontFamily.c_str(), (REAL)font->m_fontSize, FontStyleRegular| FontStyleStrikeout, UnitPixel);
			}
		}
		else{
			if (font->m_bold && font->m_underline && font->m_italic){
				return new Gdiplus::Font(font->m_fontFamily.c_str(), (REAL)font->m_fontSize, FontStyleBold | FontStyleUnderline | FontStyleItalic, UnitPixel);
			}
			else if (font->m_bold && font->m_underline){
				return new Gdiplus::Font(font->m_fontFamily.c_str(), (REAL)font->m_fontSize, FontStyleBold | FontStyleUnderline, UnitPixel);
			}
			else if (font->m_bold && font->m_italic){
				return new Gdiplus::Font(font->m_fontFamily.c_str(), (REAL)font->m_fontSize, FontStyleBold | FontStyleItalic, UnitPixel);
			}
			else if (font->m_underline && font->m_italic){
				return new Gdiplus::Font(font->m_fontFamily.c_str(), (REAL)font->m_fontSize, FontStyleUnderline | FontStyleItalic, UnitPixel);
			}
			else if (font->m_bold){
				return new Gdiplus::Font(font->m_fontFamily.c_str(), (REAL)font->m_fontSize, FontStyleBold, UnitPixel);
			}
			else if(font->m_underline){
				return new Gdiplus::Font(font->m_fontFamily.c_str(), (REAL)font->m_fontSize, FontStyleUnderline, UnitPixel);
			}
			else if (font->m_italic){
				return new Gdiplus::Font(font->m_fontFamily.c_str(), (REAL)font->m_fontSize, FontStyleItalic, UnitPixel);
			}
			else{
				return new Gdiplus::Font(font->m_fontFamily.c_str(), (REAL)font->m_fontSize, FontStyleRegular, UnitPixel);
			}
		}
	}

	Pen* GdiPlusPaint::getPen(Long dwPenColor, float width, int style){
	    Color gdiColor = getGdiPlusColor(dwPenColor);
        if (!m_pen ){
            m_pen = new Pen(gdiColor, (REAL)width);
            if (style == 0){
				m_pen->SetDashStyle(DashStyleSolid);
            }
            else if (style == 1){
                m_pen->SetDashStyle(DashStyleDash);
            }
            else if (style == 2){
                m_pen->SetDashStyle(DashStyleDot);
            }
            m_penWidth = width;
            m_penStyle = style;
			if (m_opacity == 1){
                m_penColor = dwPenColor;
            }
            else{
                m_penColor = FCColor_None;
            }
			switch(m_startLineCap){
			case 0:
				m_pen->SetStartCap(LineCapFlat);
				break;
			case 1:
				m_pen->SetStartCap(LineCapSquare);
				break;
			case 2:
				m_pen->SetStartCap(LineCapRound);
				break;
			case 3:
				m_pen->SetStartCap(LineCapTriangle);
				break;
			case 4:
				m_pen->SetStartCap(LineCapNoAnchor);
				break;
			case 5:
				m_pen->SetStartCap(LineCapSquareAnchor);
				break;
			case 6:
				m_pen->SetStartCap(LineCapRoundAnchor);
				break;
			case 7:
				m_pen->SetStartCap(LineCapDiamondAnchor);
				break;
			case 8:
				m_pen->SetStartCap(LineCapArrowAnchor);
				break;
			case 9:
				m_pen->SetStartCap(LineCapAnchorMask);
				break;
			case 10:
				m_pen->SetStartCap(LineCapCustom);
				break;
			}
			switch(m_endLineCap){
			case 0:
				m_pen->SetEndCap(LineCapFlat);
				break;
			case 1:
				m_pen->SetEndCap(LineCapSquare);
				break;
			case 2:
				m_pen->SetEndCap(LineCapRound);
				break;
			case 3:
				m_pen->SetEndCap(LineCapTriangle);
				break;
			case 4:
				m_pen->SetEndCap(LineCapNoAnchor);
				break;
			case 5:
				m_pen->SetEndCap(LineCapSquareAnchor);
				break;
			case 6:
				m_pen->SetEndCap(LineCapRoundAnchor);
				break;
			case 7:
				m_pen->SetEndCap(LineCapDiamondAnchor);
				break;
			case 8:
				m_pen->SetEndCap(LineCapArrowAnchor);
				break;
			case 9:
				m_pen->SetEndCap(LineCapAnchorMask);
				break;
			case 10:
				m_pen->SetEndCap(LineCapCustom);
				break;
			}
        }
        else{
            if (m_penColor == FCColor_None || m_penColor != dwPenColor){
                m_pen->SetColor(gdiColor);
                m_penColor = dwPenColor;
            }
			if(m_opacity != 1){
				m_penColor = FCColor_None;
			}
            if (m_penWidth != width){
                m_pen->SetWidth((REAL)width);
                m_penWidth = width;
            }
            if (m_penStyle != style){
				if (style == 0){
					m_pen->SetDashStyle(DashStyleSolid);
				}
				else if (style == 1){
					m_pen->SetDashStyle(DashStyleDash);
				}
				else if (style == 2){
					m_pen->SetDashStyle(DashStyleDot);
				}
                m_penStyle = style;
            }
        }
        return m_pen;
	}

	GraphicsPath* GdiPlusPaint::getRoundRectPath(Rect gdiPlusRect, int cornerRadius){
	    GraphicsPath *gdiPlusPath = new GraphicsPath;
        gdiPlusPath->AddArc(gdiPlusRect.X, gdiPlusRect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
		gdiPlusPath->AddLine(gdiPlusRect.X + cornerRadius, gdiPlusRect.Y, gdiPlusRect.GetRight() - cornerRadius * 2, gdiPlusRect.Y);
        gdiPlusPath->AddArc(gdiPlusRect.X + gdiPlusRect.Width - cornerRadius * 2, gdiPlusRect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
		gdiPlusPath->AddLine(gdiPlusRect.GetRight(), gdiPlusRect.Y + cornerRadius * 2, gdiPlusRect.GetRight(), gdiPlusRect.Y + gdiPlusRect.Height - cornerRadius * 2);
        gdiPlusPath->AddArc(gdiPlusRect.X + gdiPlusRect.Width - cornerRadius * 2, gdiPlusRect.Y + gdiPlusRect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
		gdiPlusPath->AddLine(gdiPlusRect.GetRight() - cornerRadius * 2, gdiPlusRect.GetBottom(), gdiPlusRect.X + cornerRadius * 2, gdiPlusRect.GetBottom());
        gdiPlusPath->AddArc(gdiPlusRect.X, gdiPlusRect.GetBottom() - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
        gdiPlusPath->AddLine(gdiPlusRect.X, gdiPlusRect.GetBottom() - cornerRadius * 2, gdiPlusRect.X, gdiPlusRect.Y + cornerRadius * 2);
        gdiPlusPath->CloseFigure();
        return gdiPlusPath;
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	GdiPlusPaint::GdiPlusPaint(){
		m_bitmap = 0;
		m_brush = 0;
		m_brushColor = 0;
		m_emptyStringFormat = 0;
		m_endLineCap = 0;
		m_g = 0;
		m_hDC = 0;
		m_offsetX = 0;
		m_offsetY = 0;
		m_opacity = 1;
		m_path = 0;
		m_pen = 0;
		m_penColor = 0;
		m_penWidth = 0;
		m_penStyle = 0;
		m_pRect.left = 0;
		m_pRect.top = 0;
		m_pRect.right = 0;
		m_pRect.bottom = 0;
		m_rotateAngle = 0;
		m_scaleFactorX = 0;
		m_scaleFactorY = 0;
		m_smoothMode = 2;
		m_startLineCap = 0;
		m_textQuality = 3;
		m_wRect.left = 0;
		m_wRect.top = 0;
		m_wRect.right = 0;
		m_wRect.bottom = 0;
	}

	GdiPlusPaint::~GdiPlusPaint(){
		if(m_g){
			delete m_g;
			m_g = 0;
		}
		if(m_bitmap){
			delete m_bitmap;
			m_bitmap = 0;
		}
		clearCaches();
	}

	void GdiPlusPaint::addArc(const FCRect& rect, float startAngle, float sweepAngle){
        int rw = rect.right - rect.left - 1;
        if (rw < 1) rw = 1;
        int rh = rect.bottom - rect.top - 1;
        if (rh < 1) rh = 1;
        Rect gdiPlusRect(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
        affectScaleFactor(&gdiPlusRect);
        m_path->AddArc(gdiPlusRect, startAngle, sweepAngle);
    }

	void GdiPlusPaint::addBezier(FCPoint *apt, int cpt){
		Point *points = new Point[cpt];
		for(int i = 0; i < cpt; i++){
			int x = apt[i].x + m_offsetX;
			int y = apt[i].y + m_offsetY;
			if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
				x = (int)(m_scaleFactorX * x);
				y = (int)(m_scaleFactorY * y);
			}
			Point newPoint(x, y);
			points[i] = newPoint;
		}
        m_path->AddBezier(points[0], points[1], points[2], points[3]);
		delete[] points;
		points = 0;
    }

	void GdiPlusPaint::addCurve(FCPoint *apt, int cpt){
		Point *points = new Point[cpt];
		for(int i = 0; i < cpt; i++){
			int x = apt[i].x + m_offsetX;
			int y = apt[i].y + m_offsetY;
			if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
				x = (int)(m_scaleFactorX * x);
				y = (int)(m_scaleFactorY * y);
			}
			Point newPoint(x, y);
			points[i] = newPoint;
		}
        m_path->AddCurve(points, cpt);
		delete[] points;
		points = 0;
    }

	void GdiPlusPaint::addEllipse(const FCRect& rect){
        int rw = rect.right - rect.left - 1;
        if (rw < 1) rw = 1;
        int rh = rect.bottom - rect.top - 1;
        if (rh < 1) rh = 1;
        Rect gdiPlusRect(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
        affectScaleFactor(&gdiPlusRect);
        m_path->AddEllipse(gdiPlusRect);
    }

	void GdiPlusPaint::addLine(int x1, int y1, int x2, int y2){
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
        m_path->AddLine(lx1, ly1, lx2, ly2);
    }

	void GdiPlusPaint::addRect(const FCRect& rect){
        int rw = rect.right - rect.left - 1;
        if (rw < 1) rw = 1;
        int rh = rect.bottom - rect.top - 1;
        if (rh < 1) rh = 1;
		Rect gdiPlusRect(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
        affectScaleFactor(&gdiPlusRect);
		m_path->AddRectangle(gdiPlusRect);
    }

	void GdiPlusPaint::addPie(const FCRect& rect, float startAngle, float sweepAngle){
        int rw = rect.right - rect.left - 1;
        if (rw < 1) rw = 1;
        int rh = rect.bottom - rect.top - 1;
        if (rh < 1) rh = 1;
		Rect gdiPlusRect(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
        affectScaleFactor(&gdiPlusRect);
        m_path->AddPie(gdiPlusRect, startAngle, sweepAngle);
    }

	void GdiPlusPaint::addText(const wchar_t *strText, FCFont *font, const FCRect& rect){
		if(!m_emptyStringFormat){
			m_emptyStringFormat = (StringFormat*)StringFormat::GenericTypographic();
			m_emptyStringFormat->SetFormatFlags(::StringFormatFlagsMeasureTrailingSpaces);
        }
		if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
			int strX = (int)(m_scaleFactorX * (rect.left + m_offsetX));
			int strY = (int)(m_scaleFactorY * (rect.top + m_offsetY));
			PointF gdiPlusPoint((REAL)strX, (REAL)strY);
			float fontSize = (float)(font->m_fontSize * (m_scaleFactorX + m_scaleFactorY) / 2);
			FCFont scaleFont(font->m_fontFamily, fontSize, font->m_bold, font->m_underline, font->m_italic);
			Gdiplus::Font *gdiFont = getFont(&scaleFont);
			FontFamily fontFamily;
			gdiFont->GetFamily(&fontFamily);
			m_path->AddString(strText, -1, &fontFamily, gdiFont->GetStyle(), gdiFont->GetSize(), gdiPlusPoint, m_emptyStringFormat);
			delete gdiFont;
			gdiFont = 0;
		}
		else{
			PointF gdiPlusPoint((REAL)(rect.left + m_offsetX), (REAL)(rect.top + m_offsetY));
			Gdiplus::Font *gdiFont = getFont(font);
			FontFamily fontFamily;
			gdiFont->GetFamily(&fontFamily);
			m_path->AddString(strText, -1, &fontFamily, gdiFont->GetStyle(), gdiFont->GetSize(), gdiPlusPoint, m_emptyStringFormat);
			delete gdiFont;
			gdiFont = 0;
		}
	}

	void GdiPlusPaint::beginExport(const String& exportPath, const FCRect& rect){
		m_exportPath = exportPath;
        int imageW = rect.right - rect.left;
        int imageH = rect.bottom - rect.top;
        if (imageW == 0) imageW = 1;
        if (imageH == 0) imageH = 1;
		if(m_bitmap){
			delete m_bitmap;
		}
		if(m_g){
			delete m_g;
		}
        m_bitmap = new Bitmap(imageW, imageH);
		m_g = Graphics::FromImage(m_bitmap);
	    setSmoothMode(m_smoothMode);
        setTextQuality(m_textQuality);
        m_opacity = 1;
        m_resourcePath = L"";
	}

	void GdiPlusPaint::beginPaint(HDC hDC, const FCRect& wRect, const FCRect& pRect){
		m_pRect = pRect;
		m_wRect = wRect;
		int width = m_wRect.right - m_wRect.left;
		int height = m_wRect.bottom - m_wRect.top;
		if(!m_bitmap || width > (int)m_bitmap->GetWidth() || height > (int)m_bitmap->GetHeight()){
			int oldWidth = 0, oldHeight = 0;
			if(m_bitmap){
				oldWidth = (int)m_bitmap->GetWidth();
				oldHeight = (int)m_bitmap->GetHeight();
				delete m_bitmap;
			}
			if(m_g){
				delete m_g;
			}
			m_bitmap = new Bitmap(max(width, oldWidth), max(height, oldHeight));
			m_g = Graphics::FromImage(m_bitmap);
		}
		m_hDC = hDC;
	    setSmoothMode(m_smoothMode);
        setTextQuality(m_textQuality);
		m_opacity = 1;
		m_resourcePath = L"";
	}

	void GdiPlusPaint::beginPath(){
		m_path = new GraphicsPath;
	}

	void GdiPlusPaint::clearCaches(){
		if(m_brush){
			delete m_brush;
			m_brush = 0;
		}
		if(m_emptyStringFormat){
			delete m_emptyStringFormat;
			m_emptyStringFormat = 0;
		}
		std::map<String, Bitmap*>::iterator sIter = m_images.begin(); 
		for(;sIter != m_images.end(); ++sIter){
			delete sIter->second;
		}
		m_images.clear();
		if(m_pen){
			delete m_pen;
			m_pen = 0;
		}
		if(m_path){
			delete m_path;
			m_path = 0;
		}
	}

	void GdiPlusPaint::clipPath(){
		m_g->SetClip(m_path);
	}

	void GdiPlusPaint::closeFigure(){
        m_path->CloseFigure();
    }

	void GdiPlusPaint::closePath(){
        delete m_path;
        m_path = 0;
    }

	void GdiPlusPaint::drawArc(Long dwPenColor, float width, int style, const FCRect& rect, float startAngle, float sweepAngle){
		if(dwPenColor == FCColor_None) return;
		int rw = rect.right - rect.left - 1;
        if (rw < 1) rw = 1;
        int rh = rect.bottom - rect.top - 1;
        if (rh < 1) rh = 1;
        Rect gdiPlusRect(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
        affectScaleFactor(&gdiPlusRect);
        m_g->DrawArc(getPen(dwPenColor, width, style), gdiPlusRect, startAngle, sweepAngle);
	}

	void GdiPlusPaint::drawBezier(Long dwPenColor, float width, int style, FCPoint *apt, int cpt){
		if(dwPenColor == FCColor_None) return;
		Point *points = new Point[cpt];
		for(int i = 0; i < cpt; i++){
			int x = apt[i].x + m_offsetX;
			int y = apt[i].y + m_offsetY;
			if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
				x = (int)(m_scaleFactorX * x);
				y = (int)(m_scaleFactorY * y);
			}
			Point newPoint(x, y);
			points[i] = newPoint;
		}
        m_g->DrawBezier(getPen(dwPenColor, width, style), points[0], points[1], points[2], points[3]);
		delete[] points;
		points = 0;
	}

	void GdiPlusPaint::drawCurve(Long dwPenColor, float width, int style, FCPoint *apt, int cpt){
		if(dwPenColor == FCColor_None) return;
		Point *points = new Point[cpt];
		for(int i = 0; i < cpt; i++){
			int x = apt[i].x + m_offsetX;
			int y = apt[i].y + m_offsetY;
			if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
				x = (int)(m_scaleFactorX * x);
				y = (int)(m_scaleFactorY * y);
			}
			Point newPoint(x, y);
			points[i] = newPoint;
		}
        m_g->DrawCurve(getPen(dwPenColor, width, style), points, cpt);
		delete[] points;
		points = 0;
	}

	void GdiPlusPaint::drawEllipse(Long dwPenColor, float width, int style, const FCRect& rect){
		if(dwPenColor == FCColor_None) return;
		int rw = rect.right - rect.left - 1;
		if(rw < 1) rw = 1;
		int rh = rect.bottom - rect.top - 1;
		if(rh < 1) rh = 1;
	    Rect gdiPlusRect(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
		affectScaleFactor(&gdiPlusRect);
        m_g->DrawEllipse(getPen(dwPenColor, width, style), gdiPlusRect);
	}

	void GdiPlusPaint::drawEllipse(Long dwPenColor, float width, int style, int left, int top, int right, int bottom){
		FCRect rect = {left, top, right, bottom};
        drawEllipse(dwPenColor, width, style, rect);
	}

	void GdiPlusPaint::drawImage(const wchar_t *imagePath, const FCRect& rect){
		String imageKey = m_resourcePath + imagePath;
        Bitmap *drawImage = 0;
		int rw = rect.right - rect.left;
		if(rw < 1) rw = 1;
		int rh = rect.bottom - rect.top;
		if(rh < 1) rh = 1;
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
			affectScaleFactor(&gdiPlusRect);
			if(m_opacity < 1){
				Gdiplus::ColorMatrix colorMatrix = {
					 1, 0, 0, 0, 0,
					 0, 1, 0, 0, 0,
					 0, 0, 1, 0, 0,
					 0, 0, 0, m_opacity, 0,
					 0, 0, 0, 0, 1
				};
				Gdiplus::ImageAttributes imageAtt;
				imageAtt.SetColorMatrix(&colorMatrix);
				m_g->DrawImage(drawImage, gdiPlusRect, 0, 0, drawImage->GetWidth(), drawImage->GetHeight(), UnitPixel, &imageAtt);
			}
			else{
				m_g->DrawImage(drawImage, gdiPlusRect, 0, 0, drawImage->GetWidth(), drawImage->GetHeight(), UnitPixel);
			}
		}
	}

	void GdiPlusPaint::drawLine(Long dwPenColor, float width, int style, const FCPoint& x, const FCPoint& y){
		drawLine(dwPenColor, width, style, x.x, x.y, y.x, y.y);
	}

	void GdiPlusPaint::drawLine(Long dwPenColor, float width, int style, int x1, int y1, int x2, int y2){
		if(dwPenColor == FCColor_None) return;
		int lx1 = x1 + m_offsetX;
        int ly1 = y1 + m_offsetY;
        int lx2 = x2 + m_offsetX;
        int ly2 = y2 + m_offsetY;
        if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
            lx1 = (int)(m_scaleFactorX * (x1 + m_offsetX));
            ly1 = (int)(m_scaleFactorY * (y1 + m_offsetY));
            lx2 = (int)(m_scaleFactorX * (x2 + m_offsetX));
            ly2 = (int)(m_scaleFactorY * (y2 + m_offsetY));
        }
		m_g->DrawLine(getPen(dwPenColor, width, style), lx1, ly1, lx2, ly2);
	}

	void GdiPlusPaint::drawPath(Long dwPenColor, float width, int style){
		if(dwPenColor == FCColor_None) return;
        m_g->DrawPath(getPen(dwPenColor, width, style), m_path);
	}

	void GdiPlusPaint::drawPie(Long dwPenColor, float width, int style, const FCRect& rect, float startAngle, float sweepAngle){
		int rw = rect.right - rect.left - 1;
        if (rw < 1) rw = 1;
        int rh = rect.bottom - rect.top - 1;
        if (rh < 1) rh = 1;
		Rect gdiPlusRect(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
        affectScaleFactor(&gdiPlusRect);
        m_g->DrawPie(getPen(dwPenColor, width, style), gdiPlusRect, startAngle, sweepAngle);
	}

	void GdiPlusPaint::drawPolygon(Long dwPenColor, float width, int style, FCPoint *apt, int cpt){
		if(dwPenColor == FCColor_None) return;
		Point *points = new Point[cpt];
		for(int i = 0; i < cpt; i++){
			int x = apt[i].x + m_offsetX;
			int y = apt[i].y + m_offsetY;
			if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
				x = (int)(m_scaleFactorX * x);
				y = (int)(m_scaleFactorY * y);
			}
			Point newPoint(x, y);
			points[i] = newPoint;
		}
		m_g->DrawPolygon(getPen(dwPenColor, width, style), points, cpt);
		delete[] points;
		points = 0;
	}

	void GdiPlusPaint::drawPolyline(Long dwPenColor, float width, int style, FCPoint *apt, int cpt){
		if(dwPenColor == FCColor_None) return;
		Point *points = new Point[cpt];
		for(int i = 0; i < cpt; i++){
			int x = apt[i].x + m_offsetX;
			int y = apt[i].y + m_offsetY;
			if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
				x = (int)(m_scaleFactorX * x);
				y = (int)(m_scaleFactorY * y);
			}
			Point newPoint(x, y);
			points[i] = newPoint;
		}
		m_g->DrawLines(getPen(dwPenColor, width, style), points, cpt);
		delete[] points;
		points = 0;
	}

	void GdiPlusPaint::drawRect(Long dwPenColor, float width, int style, int left, int top, int right, int bottom){
		FCRect rect = {left, top, right, bottom};
        drawRect(dwPenColor, width, style, rect);
	}

	void GdiPlusPaint::drawRect(Long dwPenColor, float width, int style, const FCRect& rect){
		if(dwPenColor == FCColor_None) return;
		int rw = rect.right - rect.left - 1;
		if(rw < 1) rw = 1;
		int rh = rect.bottom - rect.top - 1;
		if(rh < 1) rh = 1;
		Rect gdiPlusRect(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
		affectScaleFactor(&gdiPlusRect);
        m_g->DrawRectangle(getPen(dwPenColor, width, style), gdiPlusRect);
	}

	void GdiPlusPaint::drawRoundRect(Long dwPenColor, float width, int style, const FCRect& rect, int cornerRadius){
		if(dwPenColor == FCColor_None) return;
		int rw = rect.right - rect.left - 1;
		if(rw < 1) rw = 1;
		int rh = rect.bottom - rect.top - 1;
		if(rh < 1) rh = 1;
		Rect gdiPlusRect(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
		affectScaleFactor(&gdiPlusRect);
		if(cornerRadius != 0){
			GraphicsPath *gdiPlusPath = getRoundRectPath(gdiPlusRect, cornerRadius);
			m_g->DrawPath(getPen(dwPenColor, width, style), gdiPlusPath);
			delete gdiPlusPath;
		}
		else{
			m_g->DrawRectangle(getPen(dwPenColor, width, style), gdiPlusRect);
		}
	}

	void GdiPlusPaint::drawText(const wchar_t *strText, Long dwPenColor, FCFont *font, const FCRect& rect){
		if(dwPenColor == FCColor_None) return;
		if(!m_emptyStringFormat){
			m_emptyStringFormat = (StringFormat*)StringFormat::GenericDefault();
			m_emptyStringFormat->SetFormatFlags(::StringFormatFlagsMeasureTrailingSpaces);
        }
		if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
			int strX = (int)(m_scaleFactorX * (rect.left + m_offsetX));
			int strY = (int)(m_scaleFactorY * (rect.top + m_offsetY));
			PointF gdiPlusPoint((REAL)strX, (REAL)strY);
			float fontSize = (float)(font->m_fontSize * (m_scaleFactorX + m_scaleFactorY) / 2);
			FCFont scaleFont(font->m_fontFamily, fontSize, font->m_bold, font->m_underline, font->m_italic);
			Gdiplus::Font *gdiFont = getFont(&scaleFont);
			m_g->DrawString(strText, -1, gdiFont, gdiPlusPoint, m_emptyStringFormat, getBrush(dwPenColor));
			delete gdiFont;
			gdiFont = 0;
		}
		else{
			PointF gdiPlusPoint((REAL)(rect.left + m_offsetX), (REAL)(rect.top + m_offsetY));
			Gdiplus::Font *gdiFont = getFont(font);
			m_g->DrawString(strText, -1, gdiFont, gdiPlusPoint, m_emptyStringFormat, getBrush(dwPenColor));
			delete gdiFont;
			gdiFont = 0;
		}
	}

	void GdiPlusPaint::drawText(const wchar_t *strText, Long dwPenColor, FCFont *font, const FCRectF& rect){
		if(dwPenColor == FCColor_None) return;
		if(!m_emptyStringFormat){
			m_emptyStringFormat = (StringFormat*)StringFormat::GenericDefault();
			m_emptyStringFormat->SetFormatFlags(::StringFormatFlagsMeasureTrailingSpaces);
        }
		if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
			float strX = (float)(m_scaleFactorX * (rect.left + m_offsetX));
			float strY = (float)(m_scaleFactorY * (rect.top + m_offsetY));
			PointF gdiPlusPoint((REAL)strX, (REAL)strY);
			float fontSize = (float)(font->m_fontSize * (m_scaleFactorX + m_scaleFactorY) / 2);
			FCFont scaleFont(font->m_fontFamily, fontSize, font->m_bold, font->m_underline, font->m_italic);
			Gdiplus::Font *gdiFont = getFont(&scaleFont);
			m_g->DrawString(strText, -1, gdiFont, gdiPlusPoint, m_emptyStringFormat, getBrush(dwPenColor));
			delete gdiFont;
			gdiFont = 0;
		}
		else{
			PointF gdiPlusPoint((REAL)(rect.left + m_offsetX), (REAL)(rect.top + m_offsetY));
			Gdiplus::Font *gdiFont = getFont(font);
			m_g->DrawString(strText, -1, gdiFont, gdiPlusPoint, m_emptyStringFormat, getBrush(dwPenColor));
			delete gdiFont;
			gdiFont = 0;
		}
	}

	void GdiPlusPaint::drawTextAutoEllipsis(const wchar_t *strText, Long dwPenColor, FCFont *font, const FCRect& rect){
		if(dwPenColor == FCColor_None) return;
		if(!m_emptyStringFormat){
			m_emptyStringFormat = (StringFormat*)StringFormat::GenericDefault();
			m_emptyStringFormat->SetFormatFlags(::StringFormatFlagsMeasureTrailingSpaces);
        }
		int rw = rect.right - rect.left - 1;
		if(rw < 1) rw = 1;
		int rh = rect.bottom - rect.top - 1;
		if(rh < 1) rh = 1;
		Rect gdiPlusRect(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
		m_emptyStringFormat->SetTrimming(::StringTrimmingEllipsisCharacter);
        if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
            affectScaleFactor(&gdiPlusRect);
			float fontSize = (float)(font->m_fontSize * (m_scaleFactorX + m_scaleFactorY) / 2);
			FCFont scaleFont(font->m_fontFamily, fontSize, font->m_bold, font->m_underline, font->m_italic);
			Gdiplus::Font *gdiFont = getFont(&scaleFont);
			RectF fRect((REAL)gdiPlusRect.X, (REAL)gdiPlusRect.Y, (REAL)gdiPlusRect.Width, (REAL)gdiPlusRect.Height);
			m_g->DrawString(strText, -1, gdiFont, fRect, m_emptyStringFormat, getBrush(dwPenColor));
			delete gdiFont;
			gdiFont = 0;
        }
        else{
			RectF fRect((REAL)gdiPlusRect.X, (REAL)gdiPlusRect.Y, (REAL)gdiPlusRect.Width, (REAL)gdiPlusRect.Height);
            m_g->DrawString(strText, -1, getFont(font), fRect, m_emptyStringFormat, getBrush(dwPenColor));
        }
		m_emptyStringFormat->SetTrimming(::StringTrimmingNone);
	}

	void GdiPlusPaint::endExport(){
		if (m_bitmap){
			CLSID encoder;
			if (getEncoderClsid(L"image/jpeg", &encoder)){
				m_bitmap->Save(m_exportPath.c_str(), &encoder, 0);
			}
        }
        if (m_g){
            delete m_g;
            m_g = 0;
        }
        if (m_bitmap){
            delete m_bitmap;
            m_bitmap = 0;
        }
        m_offsetX = 0;
        m_offsetY = 0;
        m_opacity = 1;
        m_resourcePath = L"";
	}

	HBITMAP create_hbitmap_from_gdiplus_bitmap(Gdiplus::Bitmap *bitmap_ptr, Gdiplus::Rect rect){

		Gdiplus::Rect newRect(0, 0, bitmap_ptr->GetWidth(), bitmap_ptr->GetHeight());
		rect.Intersect(newRect);
		if(rect.Width > 0 && rect.Height > 0){
			BITMAP bm;
			Gdiplus::BitmapData bmpdata;
			HBITMAP hBitmap;
			if (bitmap_ptr->LockBits(&rect, Gdiplus::ImageLockModeRead, PixelFormat32bppPARGB, &bmpdata) != Gdiplus::Ok){
				return 0;
			}
			bm.bmType = 0;
			bm.bmWidth = bmpdata.Width;
			bm.bmHeight = bmpdata.Height;
			bm.bmWidthBytes = bmpdata.Stride;
			bm.bmPlanes = 1;
			bm.bmBitsPixel = 32;
			bm.bmBits = bmpdata.Scan0;
			hBitmap = CreateBitmapIndirect(&bm);
			bitmap_ptr->UnlockBits(&bmpdata);
			return hBitmap;
		}
		else{
			return 0;
		}
    }

	void GdiPlusPaint::endPaint(){
		Rect clipRect(m_pRect.left, m_pRect.top, m_pRect.right - m_pRect.left, m_pRect.bottom - m_pRect.top);
		affectScaleFactor(&clipRect);
		int width = m_wRect.right - m_wRect.left;
		int height = m_wRect.bottom - m_wRect.top;
		if(clipRect.Width < width || clipRect.Height < height){
				if(clipRect.X < m_wRect.left){
					clipRect.Width += clipRect.X;
					clipRect.X = m_wRect.left;
				}
				if(clipRect.Y < m_wRect.top){
					clipRect.Height += clipRect.Y;
					clipRect.Y = m_wRect.top;
				}
				if(clipRect.GetRight() > m_wRect.right){
					clipRect.Width -= abs(clipRect.GetRight() - m_wRect.right);
				}
				if(clipRect.GetBottom() > m_wRect.bottom){
					clipRect.Height -= abs(clipRect.GetBottom() - m_wRect.bottom);
				}
				if (clipRect.Width > 0 && clipRect.Height > 0){
					HBITMAP bitmap = create_hbitmap_from_gdiplus_bitmap(m_bitmap, clipRect);
					if(bitmap){
					HDC hdcsource = CreateCompatibleDC(m_hDC);
					SelectObject(hdcsource, bitmap);
					StretchBlt(m_hDC, clipRect.X, clipRect.Y, clipRect.Width, clipRect.Height, hdcsource, 0, 0, clipRect.Width, clipRect.Height, 13369376);
					DeleteObject(bitmap);
					DeleteObject(hdcsource);
				}
			}
		}
		else{
			if (clipRect.Width > 0 && clipRect.Height > 0){
					HBITMAP bitmap = create_hbitmap_from_gdiplus_bitmap(m_bitmap, clipRect);
					if(bitmap){
								HDC hdcsource = CreateCompatibleDC(m_hDC);
								SelectObject(hdcsource, bitmap);
								StretchBlt(m_hDC, clipRect.X, clipRect.Y, clipRect.Width, clipRect.Height, hdcsource, 0, 0, width, height, 13369376);
								DeleteObject(bitmap);
								DeleteObject(hdcsource);
					}
			}
		}
		m_hDC = 0;
		m_offsetX = 0;
		m_offsetY = 0;
		m_opacity = 1;
		m_resourcePath = L"";
	}

	void GdiPlusPaint::excludeClipPath(){
		Gdiplus::Region region(m_path);
		m_g->ExcludeClip(&region);
	}

	void GdiPlusPaint::fillEllipse(Long dwPenColor, const FCRect& rect){
		if(dwPenColor == FCColor_None) return;
		Rect gdiPlusRect(rect.left + m_offsetX, rect.top + m_offsetY, rect.right - rect.left, rect.bottom - rect.top);
		affectScaleFactor(&gdiPlusRect);
		m_g->FillEllipse(getBrush(dwPenColor), gdiPlusRect);
	}

	void GdiPlusPaint::fillGradientEllipse(Long dwFirst, Long dwSecond, const FCRect& rect, int angle){
		int rw = rect.right - rect.left - 1;
		if(rw < 1) rw = 1;
		int rh = rect.bottom - rect.top - 1;
		if(rh < 1) rh = 1;
		Rect gdiPlusRect(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
		affectScaleFactor(&gdiPlusRect);
        LinearGradientBrush lgb(gdiPlusRect, getGdiPlusColor(dwFirst), getGdiPlusColor(dwSecond), (REAL)angle, true);
        m_g->FillEllipse(&lgb, gdiPlusRect);
	}

	void GdiPlusPaint::fillGradientPath(Long dwFirst, Long dwSecond, const FCRect& rect, int angle){
		int rw = rect.right - rect.left - 1;
		if(rw < 1) rw = 1;
		int rh = rect.bottom - rect.top - 1;
		if(rh < 1) rh = 1;
		Rect gdiPlusRect(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
		affectScaleFactor(&gdiPlusRect);
        LinearGradientBrush lgb(gdiPlusRect, getGdiPlusColor(dwFirst), getGdiPlusColor(dwSecond), (REAL)angle, true);
        m_g->FillPath(&lgb, m_path);
	}

	void GdiPlusPaint::fillGradientPolygon(Long dwFirst, Long dwSecond, FCPoint *apt, int cpt, int angle){
		int left = 0, top = 0, right = 0, bottom = 0;
		Point *points = new Point[cpt];
		for(int i = 0; i < cpt; i++){
			int x = apt[i].x + m_offsetX;
			int y = apt[i].y + m_offsetY;
			if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
				x = (int)(m_scaleFactorX * x);
				y = (int)(m_scaleFactorY * y);
			}
			if (i == 0){
                left = x;
                top = y;
                right = x;
                bottom = y;
            }
            else{
                if (x < left){
                    left = x;
                }
                if (y < top){
                    top = y;
                }
                if (x > right){
                    right = x;
                }
                if (y > bottom){
                    bottom = y;
                }
            }
			Point newPoint(x, y);
			points[i] = newPoint;
		}
		Rect gdiPlusRect(left, top, right - left, bottom - top);
		if(gdiPlusRect.Height > 0){
			LinearGradientBrush lgb(gdiPlusRect, getGdiPlusColor(dwFirst), getGdiPlusColor(dwSecond), (REAL)angle, true);
			m_g->FillPolygon(&lgb, points, cpt);
		}
		delete[] points;
		points = 0;
	}

	void GdiPlusPaint::fillGradientRect(Long dwFirst, Long dwSecond, const FCRect& rect, int cornerRadius, int angle){
		int rw = rect.right - rect.left - 1;
		if(rw < 1) rw = 1;
		int rh = rect.bottom - rect.top - 1;
		if(rh < 1) rh = 1;
		Rect gdiPlusRect(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
		affectScaleFactor(&gdiPlusRect);
        LinearGradientBrush lgb(gdiPlusRect, getGdiPlusColor(dwFirst), getGdiPlusColor(dwSecond), (REAL)angle, true);
		if(cornerRadius != 0){
			GraphicsPath *gdiPlusPath = getRoundRectPath(gdiPlusRect, cornerRadius);
			m_g->FillPath(&lgb, gdiPlusPath);
			delete gdiPlusPath;
		}
		else{
			m_g->FillRectangle(&lgb, gdiPlusRect);
		}
	}

	void GdiPlusPaint::fillPath(Long dwPenColor){
		if(dwPenColor == FCColor_None) return;
		m_g->FillPath(getBrush(dwPenColor), m_path);
	}

	void GdiPlusPaint::fillPie(Long dwPenColor, const FCRect& rect, float startAngle, float sweepAngle){
		if(dwPenColor == FCColor_None) return;
		int rw = rect.right - rect.left - 1;
        if (rw < 1) rw = 1;
        int rh = rect.bottom - rect.top - 1;
        if (rh < 1) rh = 1;
		Rect gdiPlusRect(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
        affectScaleFactor(&gdiPlusRect);
        m_g->FillPie(getBrush(dwPenColor), gdiPlusRect, startAngle, sweepAngle);
	}

	void GdiPlusPaint::fillPolygon(Long dwPenColor, FCPoint *apt, int cpt){
		if(dwPenColor == FCColor_None) return;
		Point *points = new Point[cpt];
		for(int i = 0; i < cpt; i++){
			int x = apt[i].x + m_offsetX;
			int y = apt[i].y + m_offsetY;
			if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
				x = (int)(m_scaleFactorX * x);
				y = (int)(m_scaleFactorY * y);
			}
			Point newPoint(x, y);
			points[i] = newPoint;
		}
		m_g->FillPolygon(getBrush(dwPenColor), points, cpt);
		delete[] points;
		points = 0;
	}

	void GdiPlusPaint::fillRect(Long dwPenColor, const FCRect& rect){
		fillRect(dwPenColor, rect.left, rect.top, rect.right, rect.bottom);
	}

	void GdiPlusPaint::fillRect(Long dwPenColor, int left, int top, int right, int bottom){
		if(dwPenColor == FCColor_None) return;
		Rect gdiPlusRect(left + m_offsetX, top + m_offsetY, right - left, bottom - top);
		affectScaleFactor(&gdiPlusRect);
		m_g->FillRectangle(getBrush(dwPenColor), gdiPlusRect);
	}

	void GdiPlusPaint::fillRoundRect(Long dwPenColor, const FCRect& rect, int cornerRadius){
		if(dwPenColor == FCColor_None) return;
		Rect gdiPlusRect(rect.left + m_offsetX, rect.top + m_offsetY, rect.right - rect.left, rect.bottom - rect.top);
		affectScaleFactor(&gdiPlusRect);
		if(cornerRadius != 0){
			GraphicsPath *gdiPlusPath = getRoundRectPath(gdiPlusRect, cornerRadius);
			m_g->FillPath(getBrush(dwPenColor), gdiPlusPath);
			delete gdiPlusPath;
		}
		else{
			m_g->FillRectangle(getBrush(dwPenColor), gdiPlusRect);
		}
	}

	Long GdiPlusPaint::getColor(Long dwPenColor){
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

	Long GdiPlusPaint::getPaintColor(Long dwPenColor){
		return getColor(dwPenColor);
	}

	FCPoint GdiPlusPaint::getOffset(){
		FCPoint offset = {m_offsetX, m_offsetY};
		return offset;
	}

	FCPoint GdiPlusPaint::rotate(const FCPoint& op, const FCPoint& mp, int angle){
        float PI = 3.14159265f;
		FCPoint pt = {0};
        pt.x = (int)((mp.x - op.x) * cos(angle * PI / 180) - (mp.y - op.y) * sin(angle * PI / 180) + op.x);
        pt.y = (int)((mp.x - op.x) * sin(angle * PI / 180) + (mp.y - op.y) * cos(angle * PI / 180) + op.y);
        return pt;
	}

	void GdiPlusPaint::setClip(const FCRect& rect){
		Rect gdiPlusRect(rect.left + m_offsetX, rect.top + m_offsetY, rect.right - rect.left, rect.bottom - rect.top);
		if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
            gdiPlusRect.X = (int)floor(gdiPlusRect.X * m_scaleFactorX);
            gdiPlusRect.Y = (int)floor(gdiPlusRect.Y * m_scaleFactorY);
            gdiPlusRect.Width = (int)ceil(gdiPlusRect.Width * m_scaleFactorX);
            gdiPlusRect.Height = (int)ceil(gdiPlusRect.Height * m_scaleFactorY);
        }
		m_g->SetClip(gdiPlusRect);
	}

	void GdiPlusPaint::setLineCap(int startLineCap, int endLineCap){
	    m_startLineCap = startLineCap;
		m_endLineCap = endLineCap;
        if (m_pen){
			switch(m_startLineCap){
			case 0:
				m_pen->SetStartCap(LineCapFlat);
				break;
			case 1:
				m_pen->SetStartCap(LineCapSquare);
				break;
			case 2:
				m_pen->SetStartCap(LineCapRound);
				break;
			case 3:
				m_pen->SetStartCap(LineCapTriangle);
				break;
			case 4:
				m_pen->SetStartCap(LineCapNoAnchor);
				break;
			case 5:
				m_pen->SetStartCap(LineCapSquareAnchor);
				break;
			case 6:
				m_pen->SetStartCap(LineCapRoundAnchor);
				break;
			case 7:
				m_pen->SetStartCap(LineCapDiamondAnchor);
				break;
			case 8:
				m_pen->SetStartCap(LineCapArrowAnchor);
				break;
			case 9:
				m_pen->SetStartCap(LineCapAnchorMask);
				break;
			case 10:
				m_pen->SetStartCap(LineCapCustom);
				break;
			}
			switch(m_endLineCap){
			case 0:
				m_pen->SetEndCap(LineCapFlat);
				break;
			case 1:
				m_pen->SetEndCap(LineCapSquare);
				break;
			case 2:
				m_pen->SetEndCap(LineCapRound);
				break;
			case 3:
				m_pen->SetEndCap(LineCapTriangle);
				break;
			case 4:
				m_pen->SetEndCap(LineCapNoAnchor);
				break;
			case 5:
				m_pen->SetEndCap(LineCapSquareAnchor);
				break;
			case 6:
				m_pen->SetEndCap(LineCapRoundAnchor);
				break;
			case 7:
				m_pen->SetEndCap(LineCapDiamondAnchor);
				break;
			case 8:
				m_pen->SetEndCap(LineCapArrowAnchor);
				break;
			case 9:
				m_pen->SetEndCap(LineCapAnchorMask);
				break;
			case 10:
				m_pen->SetEndCap(LineCapCustom);
				break;
			}
        }
	}

	void GdiPlusPaint::setOffset(const FCPoint& offset){
		m_offsetX = offset.x;
		m_offsetY = offset.y;
	}

	void GdiPlusPaint::setOpacity(float opacity){
		m_opacity = opacity;
	}

	void GdiPlusPaint::setResourcePath(const String& resourcePath){
		m_resourcePath = resourcePath;
	}

	void GdiPlusPaint::setRotateAngle(int rotateAngle){
		m_rotateAngle = rotateAngle;
	}

	void GdiPlusPaint::setScaleFactor(double scaleFactorX, double scaleFactorY){
		m_scaleFactorX = scaleFactorX;
		m_scaleFactorY = scaleFactorY;
	}

	void GdiPlusPaint::setSmoothMode(int smoothMode){
	    m_smoothMode = smoothMode;
        if (m_g){
            switch (m_smoothMode){
                case 0:
                    m_g->SetSmoothingMode(SmoothingModeDefault);
                    break;
                case 1:
                    m_g->SetSmoothingMode(SmoothingModeAntiAlias);
                    break;
                case 2:
                    m_g->SetSmoothingMode(SmoothingModeHighQuality);
                    break;
                case 3:
                    m_g->SetSmoothingMode(SmoothingModeHighSpeed);
                    break;
            }
        }
	}

	void GdiPlusPaint::setTextQuality(int textQuality){
	    m_textQuality = textQuality;
        if (m_g){
            switch (m_textQuality){
                case 0:
                    m_g->SetTextRenderingHint(TextRenderingHintSystemDefault);
                    break;
                case 1:
                    m_g->SetTextRenderingHint(TextRenderingHintAntiAlias);
                    break;
                case 2:
                    m_g->SetTextRenderingHint(TextRenderingHintAntiAliasGridFit);
                    break;
                case 3:
                    m_g->SetTextRenderingHint(TextRenderingHintClearTypeGridFit);
                    break;
                case 4:
                    m_g->SetTextRenderingHint(TextRenderingHintSingleBitPerPixel);
                    break;
                case 5:
                    m_g->SetTextRenderingHint(TextRenderingHintSingleBitPerPixelGridFit);
                    break;
            }
        }
	}

	bool GdiPlusPaint::supportTransparent(){
		return true;
	}

	FCSize GdiPlusPaint::textSize(const wchar_t *strText, FCFont *font){
	    if(!m_emptyStringFormat){
			m_emptyStringFormat = (StringFormat*)StringFormat::GenericDefault();
			m_emptyStringFormat->SetFormatFlags(::StringFormatFlagsMeasureTrailingSpaces);
        }
		SizeF layoutSize(0, 0);
		SizeF gdiPlusSize;
		Gdiplus::Font *gdiFont = getFont(font);
        m_g->MeasureString(strText, -1, gdiFont, layoutSize, m_emptyStringFormat, &gdiPlusSize);
		FCSize size = {(long)gdiPlusSize.Width, (long)gdiPlusSize.Height};
		delete gdiFont;
		gdiFont = 0;
		return size;
	}

	FCSizeF GdiPlusPaint::textSizeF(const wchar_t *strText, FCFont *font){
		if(!m_emptyStringFormat){
			m_emptyStringFormat = (StringFormat*)StringFormat::GenericDefault();
			m_emptyStringFormat->SetFormatFlags(::StringFormatFlagsMeasureTrailingSpaces);
        }
		SizeF layoutSize(0, 0);
		SizeF gdiPlusSize;
		Gdiplus::Font *gdiFont = getFont(font);
        m_g->MeasureString(strText, -1, gdiFont, layoutSize, m_emptyStringFormat, &gdiPlusSize);
		if (wcslen(strText) == 1){
            gdiPlusSize.Width = (float)floor(gdiPlusSize.Width * 2 / 3);
        }
		FCSizeF size = {gdiPlusSize.Width, gdiPlusSize.Height};
		delete gdiFont;
		gdiFont = 0;
		return size;
	}
}