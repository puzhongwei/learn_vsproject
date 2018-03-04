// 大数相加.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include<stdio.h>  
#include<string.h>  
#define Max 101  
void print(char sum[]);  
void bigNumAdd(char a[],char b[],char sum[]);  
int main()  
{  
    char a[Max];  
    char b[Max];  
    char sum[Max];  
    gets(a);  
    gets(b);  
    bigNumAdd(a,b,sum);  
    print(sum);  
    return 0;  
}  
   
void bigNumAdd(char a[],char b[],char sum[])  
{  
    int i=0;  
    int c=0;//表示进位  
          //初始化,对以后位运算有很大帮助！  
    char m[Max]={0};  
    char n[Max]={0};  
    memset(sum,0,Max*sizeof(char)); //这里不能写成memset(sum,0,sizeof(sum));原因见注意事项1  
    //字符串反转且字符串变数字  
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
    //位运算  
    for (i=0;i<lenA||i<lenB;i++)  
    {  
        sum[i]=(m[i]+n[i]+c)%10+'0';//得到末位  
        c=(m[i]+n[i]+c)/10;//得到进位  
    }  
}  
   
void print(char sum[])  
{  
    int i=0;  
    int j=0;  
    int len = strlen(sum);  
    for (i=len-1;sum[i]==0;i--); //找到第一个不为零的位置，方便输出  
    for (j=i;j>=0;j--)  
    {  
        printf("%c",sum[j]);  
    }  
}

int _tmain(int argc, _TCHAR* argv[])
{
	return 0;
}

