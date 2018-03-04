// mysql_t.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include "iostream"
#include <stdio.h>  
#include <WinSock.h>  //一定要包含这个，或者winsock2.h  
#include "mysql.h"    //引入mysql头文件(一种方式是在vc目录里面设置，一种是文件夹拷到工程目录，然后这样包含)  
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
		 mysql_init(&m_mysql);  //连接mysql，数据库  
	}
	bool ConnectDatabase();     //函数声明  
	void FreeConnect();  
	bool QueryDatabase1();  //查询1  
	bool QueryDatabase2();  //查询2  
	bool InsertData(const char* strSql);  
	bool ModifyData(const char* strSql);  
	bool DeleteData(const char* strSql); 
public:
	MYSQL m_mysql; //mysql连接  
	MYSQL_FIELD *fd;  //字段列数组  
	char field[32][32];  //存字段名二维数组  
	MYSQL_RES *res; //这个结构代表返回行的一个查询结果集  
	MYSQL_ROW column; //一个行数据的类型安全(type-safe)的表示，表示数据行的列  
	char query[150]; //查询语句  
};
typedef shared_ptr<CSqlHelper> CSqlHelperPtr;

int _tmain(int argc, _TCHAR* argv[])
{
	CSqlHelperPtr mysql(new CSqlHelper());
	if(!mysql->ConnectDatabase())
	{
		cout<<"连接数据库失败！"<<endl;
	}
	return 0;
}

//连接数据库  
bool CSqlHelper::ConnectDatabase()  
{  
    //返回false则连接失败，返回true则连接成功  
    if (!(mysql_real_connect(&m_mysql,"localhost", "root", "123456", "databasename", 3306, NULL,0))) //中间分别是主机，用户名，密码，数据库名，端口号（可以写默认0或者3306等），可以先写成参数再传进去  
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
//释放资源  
void CSqlHelper::FreeConnect()  
{  
    //释放资源  
    mysql_free_result(res);  
    mysql_close(&m_mysql);  
}  

/***************************数据库操作***********************************/  
//其实所有的数据库操作都是先写个sql语句，然后用mysql_query(&mysql,query)来完成，包括创建数据库或表，增删改查  
//查询数据  
bool CSqlHelper::QueryDatabase1()  
{  
    sprintf(query, "select * from user"); //执行查询语句，这里是查询所有，user是表名，不用加引号，用strcpy也可以  
    mysql_query(&m_mysql,"set names gbk"); //设置编码格式（SET NAMES GBK也行），否则cmd下中文乱码  
    //返回0 查询成功，返回1查询失败  
    if(mysql_query(&m_mysql, query))        //执行SQL语句  
    {  
        printf("Query failed (%s)\n",mysql_error(&m_mysql));  
        return false;  
    }  
    else  
    {  
        printf("query success\n");  
    }  
    //获取结果集  
    if (!(res=mysql_store_result(&m_mysql)))    //获得sql语句结束后返回的结果集  
    {  
        printf("Couldn't get result from %s\n", mysql_error(&m_mysql));  
        return false;  
    }  
  
    //打印数据行数  
    printf("number of dataline returned: %d\n",mysql_affected_rows(&m_mysql));  
  
    //获取字段的信息  
    char *str_field[32];  //定义一个字符串数组存储字段信息  
    for(int i=0;i<4;i++)   //在已知字段数量的情况下获取字段名  
    {  
        str_field[i]=mysql_fetch_field(res)->name;  
    }  
    for(int i=0;i<4;i++)   //打印字段  
        printf("%10s\t",str_field[i]);  
    printf("\n");  
    //打印获取的数据  
    while (column = mysql_fetch_row(res))   //在已知字段数量情况下，获取并打印下一行  
    {  
        printf("%10s\t%10s\t%10s\t%10s\n", column[0], column[1], column[2],column[3]);  //column是列数组  
    }  
    return true;  
}  
bool CSqlHelper::QueryDatabase2()  
{  
    mysql_query(&m_mysql,"set names gbk");   
    //返回0 查询成功，返回1查询失败  
    if(mysql_query(&m_mysql, "select * from user"))        //执行SQL语句  
    {  
        printf("Query failed (%s)\n",mysql_error(&m_mysql));  
        return false;  
    }  
    else  
    {  
        printf("query success\n");  
    }  
    res=mysql_store_result(&m_mysql);  
    //打印数据行数  
    printf("number of dataline returned: %d\n",mysql_affected_rows(&m_mysql));  
    for(int i=0;fd=mysql_fetch_field(res);i++)  //获取字段名  
        strcpy(field[i],fd->name);  
    int j=mysql_num_fields(res);  // 获取列数  
    for(int i=0;i<j;i++)  //打印字段  
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
//插入数据  
bool CSqlHelper::InsertData(const char* strSql)  
{  
    //sprintf(query, "insert into user values (NULL, 'Lilei', 'wyt2588zs','lilei23@sina.cn');"); 
    if(mysql_query(&m_mysql, strSql))        //执行SQL语句  
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
//修改数据  
bool CSqlHelper::ModifyData(const char* strSql)  
{  
    //sprintf(query, "update user set email='lilei325@163.com' where name='Lilei'");  
    if(mysql_query(&m_mysql, strSql))        //执行SQL语句  
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
//删除数据  
bool CSqlHelper::DeleteData(const char *strSql)  
{  
    /*sprintf(query, "delete from user where id=6");*/  
   // char query[100];  
    //printf("please input the sql:\n");  
   // gets(query);  //这里手动输入sql语句  
    if(mysql_query(&m_mysql, strSql))        //执行SQL语句  
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