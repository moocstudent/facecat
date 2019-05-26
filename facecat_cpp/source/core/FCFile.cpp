
#include "..\\..\\stdafx.h"
#include "..\\..\\include\\core\\FCFile.h"
#include "..\\..\\include\\core\\FCStr.h"

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
		_mkdir(dir);
	}

	void FCFile::createDirectory(const String &dir){
		string sDir = FCStr::wstringTostring(dir);
		return createDirectory(sDir.c_str());
	}

	bool FCFile::isDirectoryExist(const char *dir){
		if( (_access(dir, 0 )) != -1 )
		{
			return true;
		}
		return false;
	}

	bool FCFile::isDirectoryExist(const String &dir){
		string sDir = FCStr::wstringTostring(dir);
		return isDirectoryExist(sDir.c_str());
	}

	bool FCFile::isFileExist(const char *file){
		ifstream fs;
		fs.open(file, ios::in);
		if(fs){
			fs.close();
			return true;
		}
		else{
			return false;
		}
	}

	bool FCFile::isFileExist(const String &file){
		string sFile = FCStr::wstringTostring(file);
		return isFileExist(sFile.c_str());
	}

	bool FCFile::getDirectories(const char *dir, ArrayList<string> *dirs){
		long hFile = 0;  
		struct _finddata_t fileinfo;  
		string p;  
		if((hFile = (long)_findfirst(p.assign(dir).append("\\*").c_str(),&fileinfo)) !=  -1)  {  
			do{  
				if(fileinfo.attrib &  _A_SUBDIR){  
				    if(strcmp(fileinfo.name, ".") != 0 && strcmp(fileinfo.name, "..") != 0){
						dirs->add(p.assign(dir).append("\\").append(fileinfo.name));  
					}
				}  
			}
			while(_findnext(hFile, &fileinfo)  == 0);  
			_findclose(hFile); 
		} 
		return dirs->size() > 0;
	}

	bool FCFile::getDirectories(const String &file, ArrayList<String> *dirs){
		ArrayList<string> vectors;
		String dir;
		string sFile = FCStr::wstringTostring(file);
		bool res = getDirectories(sFile.c_str(), &vectors);
		int size=(int)vectors.size();
		for (int i = 0; i < size; i++){
			dir = FCStr::stringTowstring(vectors.get(i));
			dirs->add(dir);
		}
		return res;
	}

	int FCFile::getFileLength(const char *file){
		FILE* fp = 0;
		int fileLen = 0;
		fopen_s(&fp, file, "rb");
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
		long hFile = 0;  
		struct _finddata_t fileinfo;  
		string p;  
		if((hFile = (long)_findfirst(p.assign(dir).append("\\*").c_str(),&fileinfo)) !=  -1){  
			do{  
				if(!(fileinfo.attrib &  _A_SUBDIR)){  
					files->add(p.assign(dir).append("\\").append(fileinfo.name));  
				}  
			}
			while(_findnext(hFile, &fileinfo)  == 0);  
			_findclose(hFile); 
		} 
		return files->size() > 0;
	}

	bool FCFile::getFiles(const String &dir, ArrayList<String> *files){
		String file;
		ArrayList<string> vectors;
		string sDir = FCStr::wstringTostring(dir);
		bool res = getFiles(sDir.c_str(), &vectors);
		int size=(int)vectors.size();
		for (int i = 0; i < size; i++){
			file = FCStr::stringTowstring(vectors.get(i));
			files->add(file);
		}
		return res;
	}

	int FCFile::getFileState(const char *file, struct stat *buf){
		return stat(file, buf);
	}

	int FCFile::getFileState(const String &file, struct stat *buf){
		string sFile = FCStr::wstringTostring(file);
		return getFileState(sFile.c_str(), buf);
	}

	bool FCFile::read(const char *file, string *content){
		if(FCFile::isFileExist(file)){
			int fileLength = getFileLength(file);
			char *szContent = new char[fileLength + 1];
			memset(szContent, '\0', fileLength + 1);
			ifstream fs(file, ios::in); 
			if(fs){
				if(fileLength > 0){
					while(!fs.eof()){
						fs.read(szContent, fileLength); 
					}
				}
				fs.close();
			}
			*content = szContent;
			delete[] szContent;
			szContent = 0;
			return true;
		}
		return false;
	}

	bool FCFile::read( const String &file, String *content){
		string sFile = FCStr::wstringTostring(file);
		string str;
		bool res = read(sFile.c_str(), &str);
		String temp = FCStr::stringTowstring(str);
		*content = temp;
		return res;
	}

	void FCFile::removeFile(const char *file){
		if(FCFile::isFileExist(file)){
			String wFile = FCStr::stringTowstring(file);
			::DeleteFile(wFile.c_str());
		}
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