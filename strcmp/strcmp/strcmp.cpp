// strcmp.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"

#include<stdio.h>

int strcmp(const char *str1,const char *str2)
{
	while(str1!=NULL&&str2!=NULL)
	{
		while(*str1++==*str2++)
		{
			if(*str1=='\0'&&*str2=='\0') return 1;
		}
		
		return -1; //不等的情况
	}

	return -2; //有指针为空的情况
}

int main()
{
	char *st1="abdefg";
	char *st2="abcdefg";
	printf("%d\n",strcmp(st1,st2));

	char *st3="12345";
	char *st4="12345";
	printf("%d\n",strcmp(st3,st4));

	char *st5="xyz",*st6=NULL;
	printf("%d\n",strcmp(st5,st6));

	return 0;
}
int _tmain(int argc, _TCHAR* argv[])
{
	return 0;
}

