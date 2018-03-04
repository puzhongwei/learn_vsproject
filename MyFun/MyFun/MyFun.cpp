// MyFun.cpp : 定义控制台应用程序的入口点。
// http://blog.csdn.net/ruyueyini/article/details/47448211

#include "stdafx.h"
#include "gtest/gtest.h"  
#include  "func.h"
#include <tchar.h>   //若不包含，main中参数会报错

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
    //多个测试用例时使用，如果不写，运行RUN_ALL_TESTS()时会全部测试，加上则只返回对应的测试结果  
    //testing::GTEST_FLAG(filter) = "test_case_name.test_name";
    //测试初始化
    testing::InitGoogleTest(&argc, argv);
    //return RUN_ALL_TESTS();
    RUN_ALL_TESTS();
    //暂停，方便观看结果,结果窗口将会一闪而过  
    system("PAUSE");
    return 0;
}


