// 逆序打印单链表.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"

#include<iostream>

using namespace std;

struct listnode
{
	int value;
	listnode *next;
};

void print(listnode *phead)
{
	if(phead!=NULL)
		if(phead->next!=NULL)
		{
			print(phead->next);
		}
		printf("%d\t",phead->value);
}

int _tmain(int argc, _TCHAR* argv[])
{
	listnode *p=new listnode();
	listnode *q=new listnode();
	listnode *r=new listnode();
	listnode *s=new listnode();
	p->next=q;p->value=1;
	q->next=r;q->value=2;
	r->next=s;r->value=3;
	s->value=4;
	s->next=NULL;
	print(p);
	return 0;
}

