// StlTest.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include<iostream>
#include<set>
#include<string>
#include<map>
#include<functional>
using namespace std;
int _tmain(int argc, _TCHAR* argv[])
{
	/*map<string,int> p;
	map<string,int>::iterator it;
	p.insert(pair<string,int>("pzw",10000));
	for(int i=0;i<100000;i++)
	{
		char buffer[20];   
        _itoa( i, buffer, 10 );  
        string s(buffer);  
		p.insert(pair<string,int>(s,i));
	}

	for(it=p.begin();it!=p.end();it++)
		cout<<it->first<<"    "<<it->second<<endl;
	*/
	char nts1[] = "Test";
	char nts2[] = "Test";
	string str1 (nts1);
	string str2 (nts2);

	hash<char*> ptr_hash;
	hash<string> str_hash;

	cout<<"hash value of nts1: "<<ptr_hash(nts1)<<endl;
	cout<<"hash value of nts2: "<<ptr_hash(nts2)<<endl;
	cout<<"hash value of str1: "<<str_hash(str1)<<endl;
	cout<<"hash value of str2: "<<str_hash(str2)<<endl;

	cout << "same hashes:\n" << boolalpha;
	cout << "nts1 and nts2: " << (ptr_hash(nts1)==ptr_hash(nts2)) << '\n';
	cout << "str1 and str2: " << (str_hash(str1)==str_hash(str2)) << '\n';
	return 0;
}

