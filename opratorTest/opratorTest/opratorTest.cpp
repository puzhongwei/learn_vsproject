// opratorTest.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include<iostream>
using namespace std;
class person{
private:
    int age;
    public:
    person(int a){
       this->age=a;
    }
   inline bool operator == (const person &ps) const;
};
inline bool person::operator==(const person &ps) const
{

     if (this->age==ps.age)
        return true;
     return false;
}
int _tmain(int argc, _TCHAR* argv[])
{
   person p1(20);
  person p2(20);
  if(p1==p2) cout<<"the age is equal!"<<endl;
	  return 0;
	return 0;
}

