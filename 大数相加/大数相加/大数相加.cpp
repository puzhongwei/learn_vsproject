// �������.cpp : �������̨Ӧ�ó������ڵ㡣
//

#include "stdafx.h"
#include<stdio.h>  
#include<string> 
#include <iostream>
using namespace std;
#define Max 101  
void print(char sum[]);  
void bigNumAdd(char a[],char b[],char sum[]);  
 
   
void bigNumAdd(char a[],char b[],char sum[])  
{  
    int i=0;  
    int c=0;//��ʾ��λ  
          //��ʼ��,
    char m[Max]={0};  
    char n[Max]={0};  
    memset(sum,0,Max*sizeof(char)); //���ﲻ��д��memset(sum,0,sizeof(sum));ԭ���ע������1  
    //�ַ�����ת���ַ���������  
    int lenA=strlen(a);  
    int lenB=strlen(b);  
    for (i=0;i<lenA;i++)  
    {  
        m[i]=a[lenA-i-1]-'0';  
    }  
    for (i=0;i<lenB;i++)  
    {  
        n[i]=b[lenB-i-1]-'0';  
    }  
    //λ����  
    for (i=0;i<lenA||i<lenB;i++)  
    {  
        sum[i]=(m[i]+n[i]+c)%10+'0';//�õ�ĩλ  
        c=(m[i]+n[i]+c)/10;//�õ���λ  
    }  
}  
   
void print(char sum[])  
{  
    int i=0;  
    int j=0;  
    int len = strlen(sum);  
    for (i=len-1;sum[i]==0;i--); //�ҵ���һ����Ϊ���λ�ã��������  
    for (j=i;j>=0;j--)  
    {  
        printf("%c",sum[j]);  
    }  
	cout << endl;
}

void BidMut(char a[], char b[])
{
	int la = strlen(a);
	int lb = strlen(b);
	int n[100] = { 0 };
	int m[100] = { 0 };
	char res[101];
	memset(res, 0, 101 * sizeof(char));
	for (int i = 0; i< la; ++i)
	{
		n[i] = a[la - i -1] - '0';
	}
	for (int j = 0; j< lb; ++j)
	{
		m[j] = b[lb - j - 1] - '0';
	}
	res[0] = '0';
	for (int i = 0; i < la; ++i)
	{
		char tmp[101];
		memset(tmp, 0, 101 * sizeof(char));
		int k = 0;
		int c = 0;
		int t = i;
		while (t--)
		{
			tmp[k++] = '0';
		}
		for (int j = 0; j < lb; ++j)
		{
			tmp[k++] = (n[i] * m[j] + c) % 10 + '0';
			c = (n[i] * m[j] + c) / 10;
		}
		//tmp[k] = '0'; // *10
		//BigNumAdd(tmp, res, res);
		//print(tmp);
		// ���� ��ӵĽ��
		int l1 = strlen(tmp);
		int l2 = strlen(res);
		int n2[100] = { 0 };
		int m2[100] = { 0 };
		//memset(res, 0, 101 * sizeof(char));
		for (int i = 0; i< l1; ++i)
		{
			n2[i] = tmp[i] - '0';
		}
		for (int j = 0; j< l2; ++j)
		{
			m2[j] = res[j] - '0';
		}
		int k2 = 0;
		int c2 = 0;
		while (k2 < l1 || k2 < l2)
		{
			res[k2] = (n2[k2] + m2[k2] + c2) % 10 + '0';
			c2 = (n2[k2] + m2[k2] + c2) / 10;
			k2++;
		}
		
	}
	print(res);
}

int _tmain(int argc, _TCHAR* argv[])
{
	char a[Max] ;
	char b[Max] ;
	char sum[Max];
	while (true)
	{
		gets(a);
		gets(b);
		BidMut(a, b);
	}
	//gets(a);
	//gets(b);
	//bigNumAdd(a, b, sum);
	//print(sum);
	BidMut(a, b);
	return 0;
}

