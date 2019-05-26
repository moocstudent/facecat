#include "stdafx.h"
#include "FCFile.h"

namespace FaceCat{
    bool FCFile::append(const char *file, const char *content){
        ofstream fs(file, ios::app);
        if(fs){
            fs << content;
            fs.close();
            return true;
        }
        return false;
    }
    
    bool FCFile::append(const String &file, const String &content){
        string sFile = FCStr::wstringTostring(file);
        string sContent = FCStr::wstringTostring(content);
        return append(sFile.c_str(), sContent.c_str());
    }
    
    void FCFile::createDirectory(const char *dir){
        mkdir(dir, S_IRWXU | S_IRWXG | S_IROTH | S_IXOTH);
    }
    
    void FCFile::createDirectory(const String &dir){
        string sDir = FCStr::wstringTostring(dir);
    }
    
    void FCFile::copyFileLinux(const string &srcDirPath, const string &desDirPath){
        string thisdesDirPath;
        string srcDir;
        int n = 0;
        while (srcDirPath.find('/', n) != std::string::npos){
            n = srcDirPath.find('/', n) + 1;
        }
        if(n == 0){
            std::cout << "src path error" << std::endl;
            return;
        }
        srcDir = srcDirPath.substr(n-1, srcDirPath.size());
        thisdesDirPath = desDirPath + srcDir;
        if(!makeDir(thisdesDirPath)){
            return;
        }
        ArrayList<string> fileNameList;
        if(!getFileName(srcDirPath, &fileNameList)){
            return;
        }
        if(fileNameList.size() == 0){
            return;
        }
        doCopy(srcDirPath,desDirPath, &fileNameList);
    }
    
    void FCFile::doCopy(string srcDirPath , string desDirPath ,ArrayList<string> *fileNameList)
    {
#pragma omp parallel for
        for (int i = 0; i < fileNameList->size(); i++)
        {
            string nowSrcFilePath, nowDesFilePath ;
            nowSrcFilePath = srcDirPath + "/" + fileNameList->get(i);
            nowDesFilePath = desDirPath + "/" + fileNameList->get(i);
            std::ifstream in;
            in.open(nowSrcFilePath);
            if(!in)
            {
                continue;
            }
            std::ofstream out;
            out.open(nowDesFilePath);
            if(!out)
            {
                in.close();
                continue;
            }
            out << in.rdbuf();
            out.close();
            in.close();
        }
    }
    
    bool FCFile::isDirectoryExist(const char *dir)
    {
        if( (access(dir, 0 )) != -1 )
        {
            return true;
        }
        return false;
    }
    
    bool FCFile::isFileExist(const char *file)
    {
        ifstream fs;
        fs.open(file, ios::in);
        if(fs)
        {
            fs.close();
            return true;
        }
        else
        {
            return false;
        }
    }
    
    bool FCFile::isFileExist(const String &file)
    {
        string sFile = FCStr::wstringTostring(file);
        return isFileExist(sFile.c_str());
    }
    
    bool FCFile::isDirectoryExist(const String &dir)
    {
        string sDir = FCStr::wstringTostring(dir);
        return isDirectoryExist(sDir.c_str());
    }
    
    bool FCFile::getDirectories(const char *dir, ArrayList<string> *dirs)
    {
        return false;
    }
    
    bool FCFile::getDirectories(const String &dir, ArrayList<String> *dirs)
    {
        String wDir;
        ArrayList<string> vectors;
        string sDir = FCStr::wstringTostring(dir);
        bool res = getDirectories(sDir.c_str(), &vectors);
        int size=(int)vectors.size();
        for (int i = 0; i < size; i++)
        {
            wDir = FCStr::stringTowstring(vectors.get(i));
            dirs->add(wDir);
        }
        return res;
    }
    
    int FCFile::getFileLength(const char *file){
        FILE* fp = 0;
        int fileLen = 0;
        fp = fopen(file, "rb");
        if (!fp){
            return 0;
        }
        fseek(fp, 0, SEEK_END);
        fileLen = ftell(fp);
        fclose(fp);
        return fileLen;
    }
    
    int FCFile::getFileLength(const String &file){
        string sFile = FCStr::wstringTostring(file);
        return getFileLength(sFile.c_str());
    }
    
    bool FCFile::getFiles(const char *dir, ArrayList<string> *files){
        return false;
    }
    
    bool FCFile::getFiles(const String &dir, ArrayList<String> *files){
        String wFile;
        ArrayList<string> vectors;
        string sDir = FCStr::wstringTostring(dir);
        bool res = getFiles(sDir.c_str(), &vectors);
        int size=(int)vectors.size();
        for (int i = 0; i < size; i++){
            wFile = FCStr::stringTowstring(vectors.get(i));
            files->add(wFile);
        }
        return res;
    }
    
    int FCFile::getFileState(const char *file, struct stat *buf){
        return 0;
    }
    
    int FCFile::getFileState(const String &file, struct stat *buf){
        string sFile = FCStr::wstringTostring(file);
        return getFileState(sFile.c_str(), buf);
    }
    
    bool FCFile::getFileName(string srcDirPath , ArrayList<string> *fileNameList)
    {
        DIR *dir;
        struct dirent *ptr;
        if ((dir=opendir(srcDirPath.c_str())) == NULL){
            return false;
        }
        while ((ptr=readdir(dir)) != NULL){
            if (ptr->d_name == string(".") || ptr->d_name == string(".."))
                continue;
            else if(ptr->d_type == 8)  //file
                fileNameList->add(ptr->d_name);
            else if(ptr->d_type == 10)  //link file
                continue;
            else if(ptr->d_type == 4)  //dir
                fileNameList->add(ptr->d_name);
        }
        closedir(dir);
        return true;
    }
    
    bool FCFile::makeDir (const std::string& pathName){
        if(::mkdir(pathName.c_str(), S_IRWXU | S_IRGRP | S_IXGRP) < 0){
            return false;
        }
        return true;
    }
    
    bool FCFile::read(const char *file, string *content){
        int fileLength = getFileLength(file);
        char *str = new char[fileLength];
        memset(str, '\0', fileLength);
        ifstream fs(file, ios::in);
        if(fs){
            while(!fs.eof()){
                fs.read(str, getFileLength(file));
            }
            fs.close();
            return true;
        }
        *content = str;
        delete[] str;
        str = 0;
        return false;
    }
    
    bool FCFile::read(const String &file, String *content){
        string sFile = FCStr::wstringTostring(file);
        string outStr;
        bool str = read(sFile.c_str(), &outStr);
        String temp = FCStr::stringTowstring(outStr);
        *content=temp;
        return str;
    }
    
    void FCFile::removeFile(const char *file){
        
    }
    
    void FCFile::removeFile(const String &file){
        string sFile = FCStr::wstringTostring(file);
        return removeFile(sFile.c_str());
    }
    
    bool FCFile::write(const char *file, const char *content){
        ofstream fs(file, ios::out);
        if(fs){
            fs << content;
            fs.close();
            return true;
        }
        return false;
    }
    
    bool FCFile::write(const String &file, const String &content){
        string sFile = FCStr::wstringTostring(file);
        string sContent = FCStr::wstringTostring(content);
        return write(sFile.c_str(), sContent.c_str());
    }
}
