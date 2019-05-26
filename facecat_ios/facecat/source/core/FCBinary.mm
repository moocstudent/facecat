#include "stdafx.h"
#include "FCBinary.h"
#include <sys/mman.h>

namespace FaceCat{
    unsigned int FCBinary::m_dwPageSize = 0;
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    unsigned int FCBinary::deAllocateBuffer(unsigned int nRequestedSize){
        if(nRequestedSize == 0){
            return 0;
        }
        if(m_bSustainSize){
            return 0;
        }
        if(m_nSize <= m_nInitSize){
            return 0;
        }
        if(nRequestedSize < m_nDataSize){
            return 0;
        }
        if(nRequestedSize < m_nInitSize){
            nRequestedSize = m_nInitSize;
        }
        unsigned int nNewSize = m_nSize;
        while(nNewSize >= nRequestedSize * 2){
            nNewSize /= 2;
        }
        if(nNewSize == m_nSize){
            return 0;
        }
        PBYTE pNewBuffer = (PBYTE)valloc(nNewSize);
        mprotect(pNewBuffer, nNewSize, PROT_READ|PROT_EXEC|PROT_WRITE);
        if(m_pBase){
            if(m_nDataSize){
                memcpy(pNewBuffer, m_pBase, m_nDataSize);
            }
            free(m_pBase);
        }
        m_pBase = pNewBuffer;
        m_nSize = nNewSize;
        return m_nSize;
    }
    
    unsigned int FCBinary::getMemSize(){
        return m_nSize;
    }
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    FCBinary::FCBinary(){
        m_bSingleRead = true;
        m_bSustainSize = false;
        m_nDataSize = 0;
        m_nInitSize = 0;
        m_nReadPos = 0;
        m_nSize = 0;
        m_pBase =  0;
        if(m_dwPageSize == 0){
            int dwPageSize = 4096;
            m_dwPageSize = dwPageSize;
            while(m_dwPageSize < 8192){
                m_dwPageSize += dwPageSize;
            }
        }
    }
    
    FCBinary::~FCBinary(){
        if(m_pBase){
            free(m_pBase);
        }
    }
    
    const FCBinary& FCBinary::operator+(FCBinary& buff){
        this->write(buff.getBytes(), buff.getBytesLen());
        return *this;
    }
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void FCBinary::clearBytes(){
        m_nDataSize = 0;
        m_nReadPos = 0;
        deAllocateBuffer(0);
    }
    
    void FCBinary::copy(FCBinary& buffer){
        unsigned int nReSize = buffer.getMemSize();
        if(nReSize != m_nSize){
            if (m_pBase){
                free(m_pBase);
            }
            m_pBase = (PBYTE)valloc(nReSize);
            mprotect(m_pBase, nReSize, PROT_READ|PROT_EXEC|PROT_WRITE);
            m_nSize = nReSize;
        }
        m_nDataSize = buffer.getBytesLen();
        if(m_nDataSize > 0){
            memcpy(m_pBase, buffer.getBytes(), m_nDataSize);
        }
    }
    
    unsigned int FCBinary::deleteBuffer(unsigned int nSize){
        if(nSize == 0){
            return nSize;
        }
        if (nSize > m_nDataSize){
            nSize = m_nDataSize;
        }
        m_nDataSize -= nSize;
        if(m_nDataSize > 0){
            memmove(m_pBase, m_pBase+nSize, m_nDataSize);
        }
        deAllocateBuffer(m_nDataSize);
        return nSize;
    }
    
    unsigned int FCBinary::deleteEnd(unsigned int nSize){
        if(nSize > m_nDataSize)
            nSize = m_nDataSize;
        if(nSize){
            m_nDataSize -= nSize;
            deAllocateBuffer(m_nDataSize);
        }
        return nSize;
    }
    
    void FCBinary::fileRead(const String& strFileName){
    }
    
    void FCBinary::fileWrite(const String& strFileName){
    }
    
    PBYTE FCBinary::getBytes(unsigned int nPos){
        if(!m_pBase){
            return 0;
        }
        return m_pBase + nPos;
    }
    
    unsigned int FCBinary::getBytesLen(){
        if(!m_pBase){
            return 0;
        }
        if (m_bSingleRead){
            return m_nDataSize - m_nReadPos;
        }
        else{
            return m_nDataSize;
        }
    }
    
    void FCBinary::initialize(unsigned int nInitsize, bool bSustain){
        m_bSustainSize = bSustain;
        reAllocateBuffer(nInitsize);
        m_nInitSize = m_nSize;
    }
    
    unsigned int FCBinary::insert(const void *pData, unsigned int nSize){
        reAllocateBuffer(nSize + m_nDataSize);
        memmove(m_pBase + nSize, m_pBase, m_nDataSize);
        memcpy(m_pBase, pData, nSize);
        m_nDataSize += nSize;
        return nSize;
    }
    
    unsigned int FCBinary::insert(String& strData){
        int nSize = (int)strData.size();
        return insert((const void*) strData.c_str(), nSize);
    }
    
    unsigned int FCBinary::read(void *pData, unsigned int nSize){
        if (nSize){
            if (m_bSingleRead){
                if (nSize+m_nReadPos > m_nDataSize){
                    throw DATA_LACK;
                    return 0;
                }
                memcpy(pData, m_pBase + m_nReadPos, nSize);
                m_nReadPos += nSize;
            }
            else{
                if (nSize > m_nDataSize){
                    throw DATA_LACK;
                    return 0;
                }
                m_nDataSize -= nSize;
                memcpy(pData, m_pBase, nSize);
                if (m_nDataSize > 0){
                    memmove(m_pBase, m_pBase + nSize, m_nDataSize);
                }
            }
        }
        deAllocateBuffer(m_nDataSize);
        return nSize;
    }
    
    char FCBinary::readChar(){
        char cValue;
        read(&cValue, sizeof(char));
        return cValue;
    }
    
    double FCBinary::readDouble(){
        double dValue;
        read(&dValue, sizeof(double));
        return dValue;
    }
    
    float FCBinary::readFloat(){
        float fValue;
        read(&fValue, sizeof(float));
        return fValue;
    }
    
    int FCBinary::readInt(){
        int nValue;
        read(&nValue, sizeof(int));
        return nValue;
    }
    
    Long FCBinary::readLong(){
        return 0;
    }
    
    short FCBinary::readShort(){
        short sValue;
        read(&sValue, sizeof(short));
        return sValue;
    }
    
    String FCBinary::readString(){
        String strData;
        unsigned int dwSize;
        read(&dwSize, sizeof(unsigned int));
        if(dwSize == 0){
            return strData;
        }
        if (m_bSingleRead){
            if (dwSize + m_nReadPos > m_nDataSize){
                throw DATA_LACK;
                return 0;
            }
            string str((const char*)m_pBase+m_nReadPos, dwSize);
            strData = FCStr::stringTowstring(str);
            m_nReadPos += dwSize;
        }
        else{
            if (dwSize > m_nDataSize){
                throw DATA_LACK;
                return 0;
            }
            string str((const char*)m_pBase,dwSize);
            strData = FCStr::stringTowstring(str.c_str());
            deleteBuffer(dwSize);
        }
        return strData;
    }
    
    string FCBinary::readString2(){
        string strData;
        unsigned int dwSize;
        read(&dwSize, sizeof(unsigned int));
        if(dwSize == 0){
            return strData;
        }
        if (m_bSingleRead){
            if (dwSize+m_nReadPos > m_nDataSize){
                throw DATA_LACK;
                return 0;
            }
            string str((const char*)m_pBase+m_nReadPos,dwSize);
            strData = str;
            m_nReadPos += dwSize;
        }
        else{
            if (dwSize > m_nDataSize){
                throw DATA_LACK;
                return 0;
            }
            string str((const char*)m_pBase,dwSize);
            strData = str;
            deleteBuffer(dwSize);
        }
        return strData;
    }
    
    unsigned int FCBinary::reAllocateBuffer(unsigned int nRequestedSize){
        if(nRequestedSize <= m_nSize){
            return 0;
        }
        unsigned int nNewSize = m_nSize;
        if(nNewSize < m_dwPageSize){
            nNewSize = m_dwPageSize;
        }
        while(nRequestedSize > nNewSize){
            nNewSize *= 2;
        }
        PBYTE pNewBuffer = (PBYTE)valloc(nNewSize);
        mprotect(pNewBuffer, nNewSize, PROT_READ|PROT_EXEC|PROT_WRITE);
        if (m_pBase){
            if(m_nDataSize){
                memcpy(pNewBuffer, m_pBase, m_nDataSize);
            }
            free(m_pBase);
        }
        m_pBase = pNewBuffer;
        m_nSize = nNewSize;
        return m_nSize;
    }
    
    unsigned int FCBinary::skipData(int nSize){
        if(m_bSingleRead){
            if (nSize + m_nReadPos > m_nDataSize){
                throw DATA_LACK;
                return 0;
            }
            m_nReadPos += nSize;
            return nSize;
        }
        else{
            m_nDataSize -= nSize;
            if (m_nDataSize > 0){
                memmove(m_pBase, m_pBase + nSize, m_nDataSize);
            }
            deAllocateBuffer(m_nDataSize);
        }
        return 0;
    }
    
    unsigned int FCBinary::write(const void *pData, unsigned int nSize){
        if(nSize){
            reAllocateBuffer(nSize + m_nDataSize);
            memcpy(m_pBase + m_nDataSize, pData, nSize);
            m_nDataSize += nSize;
        }
        return nSize;
    }
    
    unsigned int FCBinary::write(String& strData){
        int nSize = (int)strData.size();
        return write((const void*)strData.c_str(), nSize);
    }
    
    void FCBinary::writeChar(char cValue){
        write(&cValue, sizeof(char));
    }
    
    void FCBinary::writeDouble(double dValue){
        write(&dValue, sizeof(double));
    }
    
    void FCBinary::writeFloat(float fValue){
        write(&fValue, sizeof(float));
    }
    
    void FCBinary::writeInt(int nValue){
        write(&nValue, sizeof(int));
    }
    
    void FCBinary::writeLong(Long hValue){
    }
    
    void FCBinary::writeShort(short sValue){
        write(&sValue, sizeof(short));
    }
    
    unsigned int FCBinary::writeString(const String& strData){
        string str = FCStr::wstringTostring(strData);
        unsigned int dwSize = (int)str.size();
        write(&dwSize, sizeof(unsigned int));
        if(dwSize){
            write((const void*) str.c_str(), dwSize);
        }
        return (unsigned int)(dwSize + sizeof(unsigned int));
    }
    
    unsigned int FCBinary::writeString(const string& strData){
        unsigned int wSize = (int)strData.size();
        write(&wSize, sizeof(unsigned int));
        if(wSize){
            write((const void*) strData.c_str(), wSize);
        }
        return (unsigned int)(wSize + sizeof(unsigned int));
    }
}
