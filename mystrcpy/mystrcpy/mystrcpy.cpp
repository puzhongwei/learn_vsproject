// mystrcpy.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include<stdio.h>
#include<malloc.h>
#include<assert.h>
#include<string.h>
void stringcpy(char *to,const char *from)
{
	assert(to!=NULL&&from!=NULL);
	while(*from!='\0')
	{
		*to++=*from++;
	}
	*to='\0';

}

int _tmain(int argc, _TCHAR* argv[])
{
	char *f;
	char *t;
	f=(char *)malloc(15);
	t=(char *)malloc(15);
	stringcpy(f,"asdfghjkl");
	stringcpy(t,f);
	printf("%s\n",f);
	printf("%s\n",t);
	return 0;
}

