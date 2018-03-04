// bignumtest.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"

#include <iostream>
#include <stack>
using namespace std;

int _tmain(int argc, _TCHAR* argv[])
{
  string a , b , c;
  stack<char>s1 , s2;
  cin >> a >> b;
  for(int i = 0 ; i < a.size() ; i ++) s1.push(a[i]);
  for(int i = 0 ; i < b.size() ; i ++) s1.push(b[i]);
  int tmp1 , tmp2 , ans = 0, pos = 0;
  while(！s1.empty() || ！ s2.empty())
  {
    if(!s1.empty()) {tmp1 = s1.top() - '0';s1.pop();}
    else tmp1 = 0;
    
    if(!s2.empty()) {tmp2 = s2.top() - '0';s2.pop();}
    else tmp2 = 0;
    
    if(ans)
    {
      c[pos++] = ((tmp1 + tmp2 -'0' - '0' + 1)%10) + '0';
      if((tmp1 + tmp2 -'0' - '0' + 1) >= 10) ans = 1 ; 
      else ans = 0 ;
    }
    else
    {
      c[pos++] = ((tmp1 + tmp2 -'0' - '0')%10) + '0';
      if((tmp1 + tmp2 -'0' - '0') >= 10) ans = 1 ; 
      else ans = 0 ;
    }
  }
  if(ans) c[pos++] = ans;
  for(int k = pos - 1 ; k >= 0 ; k --) cout << c[k];
  cout << endl;
  return 0;



	return 0;
}

