// MyFun.cpp : �������̨Ӧ�ó������ڵ㡣
// http://blog.csdn.net/ruyueyini/article/details/47448211

#include "stdafx.h"
#include "gtest/gtest.h"  
#include  "func.h"
#include <tchar.h>   //����������main�в����ᱨ��

TEST(fun, case1)
{
    EXPECT_LT(-2, fun(1, 2));
    EXPECT_EQ(-1, fun(1, 2));
    ASSERT_LT(-2, fun(1, 2));
    ASSERT_EQ(-1, fun(1, 2));
}

TEST(fun, case2)
{
    EXPECT_LT(-2, fun(1, 2));
    EXPECT_EQ(-1, fun(1, 2));
    ASSERT_LT(-2, fun(1, 2));
    ASSERT_EQ(-1, fun(1, 2));
}
TEST(fun, case3)
{
    EXPECT_LT(-2, fun(1, 2));
    EXPECT_EQ(-1, fun(1, 2));
    ASSERT_LT(-2, fun(1, 2));
    ASSERT_EQ(-1, fun(1, 2));
}
TEST(fun, case4)
{
    EXPECT_LT(-2, fun(1, 2));
    EXPECT_EQ(-1, fun(1, 2));
    ASSERT_LT(-2, fun(1, 2));
    ASSERT_EQ(-1, fun(1, 2));
}
TEST(fun, case5)
{
    EXPECT_LT(-2, fun(1, 2));
    EXPECT_EQ(-1, fun(1, 2));
    ASSERT_LT(-2, fun(1, 2));
    ASSERT_EQ(-1, fun(1, 2));
}
TEST(fun, case6)
{
    EXPECT_LT(-2, fun(1, 2));
    EXPECT_EQ(-1, fun(1, 2));
    ASSERT_LT(-2, fun(1, 2));
    ASSERT_EQ(-1, fun(1, 2));
}
TEST(fun, case7)
{
    EXPECT_LT(-2, fun(1, 2));
    EXPECT_EQ(-1, fun(1, 2));
    ASSERT_LT(-2, fun(1, 2));
    ASSERT_EQ(-1, fun(1, 2));
}
int _tmain(int argc, _TCHAR* argv[])
{
    //�����������ʱʹ�ã������д������RUN_ALL_TESTS()ʱ��ȫ�����ԣ�������ֻ���ض�Ӧ�Ĳ��Խ��  
    //testing::GTEST_FLAG(filter) = "test_case_name.test_name";
    //���Գ�ʼ��
    testing::InitGoogleTest(&argc, argv);
    //return RUN_ALL_TESTS();
    RUN_ALL_TESTS();
    //��ͣ������ۿ����,������ڽ���һ������  
    system("PAUSE");
    return 0;
}


