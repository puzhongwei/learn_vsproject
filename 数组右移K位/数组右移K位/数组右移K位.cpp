// ��������Kλ.cpp : �������̨Ӧ�ó������ڵ㡣
//

#include "stdafx.h"
#include"iostream"
using namespace std;
void change(char a[],int c,int b)
{
	for(;c<b;c++,b--)
	{
		char temp=a[c];
		a[c]=a[b];
		a[b]=temp;
	}
}

void Move(char a[],int n,int k)
{
	k%=n;
	change(a,0,n-k-1);
	change(a,n-k,n-1);
	change(a,0,n-1);

}
int _tmain(int argc, _TCHAR* argv[])
{
	char a[]={'1','2','3','4','a','b','c','d'};
	int k=0;
	while(cin>>k){
	cout<<"�ƶ�"<<k<<"λ:"<<endl;
	cout<<"�ƶ�ǰ:"<<endl;
	for(int i=0;i<8;i++)
	{
		cout<<a[i];
	}
	 Move(a,8,k);
	 cout<<"�ƶ���:"<<endl;
	for(int i=0;i<8;i++)
	{
		cout<<a[i];
	}
	}
	return 0;
}

