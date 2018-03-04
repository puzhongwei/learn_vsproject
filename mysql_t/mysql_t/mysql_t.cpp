// mysql_t.cpp : �������̨Ӧ�ó������ڵ㡣
//

#include "stdafx.h"
#include "iostream"
#include <stdio.h>  
#include <WinSock.h>  //һ��Ҫ�������������winsock2.h  
#include "mysql.h"    //����mysqlͷ�ļ�(һ�ַ�ʽ����vcĿ¼�������ã�һ�����ļ��п�������Ŀ¼��Ȼ����������)  
#include <Windows.h>  
#include <string>
#include <memory>
//#include <boost\serialization\singleton.hpp>
#pragma comment(lib,"wsock32.lib")  
#pragma comment(lib,"libmysql.lib")  
using namespace std;

class CSqlHelper
{
public:
	CSqlHelper()
	{
		Init();
	}
	~CSqlHelper()
	{
		FreeConnect();
	}

public:
	void Init()
	{
		 mysql_init(&m_mysql);  //����mysql�����ݿ�  
	}
	bool ConnectDatabase();     //��������  
	void FreeConnect();  
	bool QueryDatabase1();  //��ѯ1  
	bool QueryDatabase2();  //��ѯ2  
	bool InsertData(const char* strSql);  
	bool ModifyData(const char* strSql);  
	bool DeleteData(const char* strSql); 
public:
	MYSQL m_mysql; //mysql����  
	MYSQL_FIELD *fd;  //�ֶ�������  
	char field[32][32];  //���ֶ�����ά����  
	MYSQL_RES *res; //����ṹ�������е�һ����ѯ�����  
	MYSQL_ROW column; //һ�������ݵ����Ͱ�ȫ(type-safe)�ı�ʾ����ʾ�����е���  
	char query[150]; //��ѯ���  
};
typedef shared_ptr<CSqlHelper> CSqlHelperPtr;

int _tmain(int argc, _TCHAR* argv[])
{
	CSqlHelperPtr mysql(new CSqlHelper());
	if(!mysql->ConnectDatabase())
	{
		cout<<"�������ݿ�ʧ�ܣ�"<<endl;
	}
	return 0;
}

//�������ݿ�  
bool CSqlHelper::ConnectDatabase()  
{  
    //����false������ʧ�ܣ�����true�����ӳɹ�  
    if (!(mysql_real_connect(&m_mysql,"localhost", "root", "123456", "databasename", 3306, NULL,0))) //�м�ֱ����������û��������룬���ݿ������˿ںţ�����дĬ��0����3306�ȣ���������д�ɲ����ٴ���ȥ  
    {  
        printf( "Error connecting to database:%s\n",mysql_error(&m_mysql));  
        return false;  
    }  
    else  
    {  
        printf("Connected...\n");  
        return true;  
    }  
}  
//�ͷ���Դ  
void CSqlHelper::FreeConnect()  
{  
    //�ͷ���Դ  
    mysql_free_result(res);  
    mysql_close(&m_mysql);  
}  

/***************************���ݿ����***********************************/  
//��ʵ���е����ݿ����������д��sql��䣬Ȼ����mysql_query(&mysql,query)����ɣ������������ݿ�����ɾ�Ĳ�  
//��ѯ����  
bool CSqlHelper::QueryDatabase1()  
{  
    sprintf(query, "select * from user"); //ִ�в�ѯ��䣬�����ǲ�ѯ���У�user�Ǳ��������ü����ţ���strcpyҲ����  
    mysql_query(&m_mysql,"set names gbk"); //���ñ����ʽ��SET NAMES GBKҲ�У�������cmd����������  
    //����0 ��ѯ�ɹ�������1��ѯʧ��  
    if(mysql_query(&m_mysql, query))        //ִ��SQL���  
    {  
        printf("Query failed (%s)\n",mysql_error(&m_mysql));  
        return false;  
    }  
    else  
    {  
        printf("query success\n");  
    }  
    //��ȡ�����  
    if (!(res=mysql_store_result(&m_mysql)))    //���sql�������󷵻صĽ����  
    {  
        printf("Couldn't get result from %s\n", mysql_error(&m_mysql));  
        return false;  
    }  
  
    //��ӡ��������  
    printf("number of dataline returned: %d\n",mysql_affected_rows(&m_mysql));  
  
    //��ȡ�ֶε���Ϣ  
    char *str_field[32];  //����һ���ַ�������洢�ֶ���Ϣ  
    for(int i=0;i<4;i++)   //����֪�ֶ�����������»�ȡ�ֶ���  
    {  
        str_field[i]=mysql_fetch_field(res)->name;  
    }  
    for(int i=0;i<4;i++)   //��ӡ�ֶ�  
        printf("%10s\t",str_field[i]);  
    printf("\n");  
    //��ӡ��ȡ������  
    while (column = mysql_fetch_row(res))   //����֪�ֶ���������£���ȡ����ӡ��һ��  
    {  
        printf("%10s\t%10s\t%10s\t%10s\n", column[0], column[1], column[2],column[3]);  //column��������  
    }  
    return true;  
}  
bool CSqlHelper::QueryDatabase2()  
{  
    mysql_query(&m_mysql,"set names gbk");   
    //����0 ��ѯ�ɹ�������1��ѯʧ��  
    if(mysql_query(&m_mysql, "select * from user"))        //ִ��SQL���  
    {  
        printf("Query failed (%s)\n",mysql_error(&m_mysql));  
        return false;  
    }  
    else  
    {  
        printf("query success\n");  
    }  
    res=mysql_store_result(&m_mysql);  
    //��ӡ��������  
    printf("number of dataline returned: %d\n",mysql_affected_rows(&m_mysql));  
    for(int i=0;fd=mysql_fetch_field(res);i++)  //��ȡ�ֶ���  
        strcpy(field[i],fd->name);  
    int j=mysql_num_fields(res);  // ��ȡ����  
    for(int i=0;i<j;i++)  //��ӡ�ֶ�  
        printf("%10s\t",field[i]);  
    printf("\n");  
    while(column=mysql_fetch_row(res))  
    {  
        for(int i=0;i<j;i++)  
            printf("%10s\t",column[i]);  
        printf("\n");  
    }  
    return true;  
}  
//��������  
bool CSqlHelper::InsertData(const char* strSql)  
{  
    //sprintf(query, "insert into user values (NULL, 'Lilei', 'wyt2588zs','lilei23@sina.cn');"); 
    if(mysql_query(&m_mysql, strSql))        //ִ��SQL���  
    {  
        printf("Query failed (%s)\n",mysql_error(&m_mysql));  
        return false;  
    }  
    else  
    {  
        printf("Insert success\n");  
        return true;  
    }  
}  
//�޸�����  
bool CSqlHelper::ModifyData(const char* strSql)  
{  
    //sprintf(query, "update user set email='lilei325@163.com' where name='Lilei'");  
    if(mysql_query(&m_mysql, strSql))        //ִ��SQL���  
    {  
        printf("Query failed (%s)\n",mysql_error(&m_mysql));  
        return false;  
    }  
    else  
    {  
        printf("Insert success\n");  
        return true;  
    }  
}  
//ɾ������  
bool CSqlHelper::DeleteData(const char *strSql)  
{  
    /*sprintf(query, "delete from user where id=6");*/  
   // char query[100];  
    //printf("please input the sql:\n");  
   // gets(query);  //�����ֶ�����sql���  
    if(mysql_query(&m_mysql, strSql))        //ִ��SQL���  
    {  
        printf("Query failed (%s)\n",mysql_error(&m_mysql));  
        return false;  
    }  
    else  
    {  
        printf("Insert success\n");  
        return true;  
    }  
}  