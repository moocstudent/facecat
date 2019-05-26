#include "stdafx.h"
#include "Future.h"

Future::Future(){
}

Future::~Future(){
}

void Future::load(const String& xmlPath){
    m_native = getNative();
    loadFile(xmlPath, 0);
}


