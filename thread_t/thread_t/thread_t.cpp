// thread_t.cpp : �������̨Ӧ�ó������ڵ㡣
//

#include "stdafx.h"
#include <iostream>  
#include <boost/thread.hpp>   
#include "create_thread.h"
#include "operate_thread.h"
#include "mutex_t.h"
#include <boost/uuid/uuid.hpp>
#include <boost/uuid/uuid_generators.hpp>
#include <boost/uuid/uuid_io.hpp>
#include <boost/lexical_cast.hpp>
using namespace boost::uuids;


// �������������������ź���
//1.������ �ӱ�����˵����һ����, �ṩ�Թ�����Դ�ı������ʡ�
//
//2.�������� �������̼߳乲���ȫ�ֱ�������ͬ����һ�ֻ��ƣ�
//��Ҫ��������������һ���̵߳ȴ�"������������������"������
//��һ���߳�ʹ"��������"���������������źţ���Ϊ�˷�ֹ������
//����������ʹ�����Ǻ�һ�������������һ��
//(thread���ṩ����������������condition_variable��condition_variable_any
//��һ�����������Ӧ��ʹ��condition_variable_any�����ܹ���Ӧ���㷺�Ļ��������͡�)

//3.�ź��� �����̣߳�����̣����ͬ����һ���߳������ĳһ��������ͨ���ź������߱���̣߳�
//����߳��ٽ���ĳЩ����.
using namespace boost;
using namespace boost::uuids::detail;
int main(int argc, char* argv[])  
{      
	//uuid u;
	//std::fill_n(u.data + 10, 6, 8); //��׼�㷨fill_nֱ�Ӳ�������
	//cout<<u<<endl;
	int i = lexical_cast<int> ("1234");
	cout<<i<<endl;
    return 0;  
}