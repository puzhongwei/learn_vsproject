// 字符串空格替换.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"

#include<iostream>

using namespace std;

int _tmain(int argc, _TCHAR* argv[])
{
	return 0;
}

void RepalceBlank(char string[],int leng)
{
	if(string==NULL&&leng<=0)
		return ;

	int ol=0;
	int bl=0;
	int i=0;
	while(string[i]!='\0')
	{
		++ol;
		if(string[i]==' ')
		++bl;
		++i;
	}

	//开始替换
	int newl=ol+bl*2;

	//if(newl>leng)
	//	return ;
	int q=newl;
	int p=ol;
	while(p>=0&&q>p)
	{
		if(string[p]== ' ')
		{
			string[q--]='0';
			string[q--]='2';
			string[q--]='%';
		}
		else
		{
			string[q--]=string[p];
		}
		--p;
	}

}
int main()
{
	char s[]={'w','e',' ','a','r','e',' ','h','a','p','p','y','.','\0'};
	 RepalceBlank(s,14);
	 for(int i=0;i<sizeof(s);i++)
	 {
		 cout<<s[i];
	 }
	return 0;
}