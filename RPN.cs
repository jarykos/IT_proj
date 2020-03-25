using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace projekt
{
    public class RPN
    {
        static Dictionary<string,int> D = new Dictionary<string, int>()
        {
            {"abs",4},{"cos",4},{"exp",4},{"log",4},{"sin",4},{"sqrt",4},{"tan",4},{"tanh",4},{"acos",4},{"asin",4},{"atan",4},
            {"^",3},
            {"*",2},{"/",2},
            {"+",1},{"-",1},
            {"(",0}
        };

        double zmienna,min,max;
        int krok, minus;
        string fun;
        List<string>L = new List<string>();
        public Stack<string> S = new Stack<string>();
        public Queue<string> Q = new Queue<string>();

        public RPN(string funkcja, double x, double xMin, double xMax, int n)
        {
            this.fun = funkcja;
            this.zmienna = x;
            this.min = xMin;
            this.max = xMax;
            this.krok = n;

            Regex rg = new Regex(@"\.");
            this.fun = rg.Replace(this.fun,",");

            if(this.fun[0]=='-' && this.fun[1]!='(') //pierwszy minus
            {
                this.fun = "0"+this.fun;
            }
            else if(this.fun[0]=='-' && this.fun[1]=='(')// pierwszy minus i cała funkcja w nawiasie
            {
                this.minus = 1;
                this.fun = this.fun.Remove(0,1);
            }

        }

        public string[] Tokeny()
        {
            Regex regex = new Regex(@"\(|\)|\^|\*|\/|\+|\-|(abs)|(cos)|(exp)|(log)|(sin)|(sqrt)|(tan)|(cosh)|(sinh)|(tanh)|(acos)|(asin)|(atan)|(x)|((\d*)(\,)?(\d+))");
            MatchCollection tokeny = regex.Matches(this.fun);
            string[] tabTokeny = new string[tokeny.Count];
            int i = 0;

            

            foreach(Match tok in tokeny)
            {
                tabTokeny[i] = tok.Value;
                i++;
            }
            

            return tabTokeny;
        }

        public void Walidacja()
        {
            int nawias = 0;
            string[] token= new string[this.Tokeny().Length];
            token = this.Tokeny();
            
            

            for(int i = 0; i < token.Length; i++)
            {
                if(D.ContainsKey(token[i]))
                {
                    if(D.ContainsKey(token[i+1]) && (D[token[i]]==1 || D[token[i]]==2 || D[token[i]]==3) && (D[token[i+1]]==1 || D[token[i+1]]==2 || D[token[i+1]]==3))
                    {
                        Console.WriteLine("niepoprawnie wprowadzone operatory matematyczne");
                        Environment.Exit(0);
                    }
                    else if(D[token[i]]==4 && token[i+1]!="(")
                    {
                        Console.WriteLine("źle urzyta funkcja matematyczna");
                        Environment.Exit(0);
                    }
                    
                }

                if(token[i]=="(") 
                {
                   nawias++;
                }
                else if(token[i]==")") 
                {
                    nawias--;
                }
                
            }

            if(nawias!=0){
                Console.WriteLine("niepoprawna ilość nawiasów");
                Environment.Exit(0);
            }

            foreach(string t in token)
            {
                Console.Write("{0} ",t);
            }
            Console.Write("\n");
        }

        public void Postfix()
        {
            double tok;
            foreach(string token in this.Tokeny())
            {
                if(token=="(")
                    S.Push(token);
                else if(token==")")
                {
                    while(S.Peek()!="(")Q.Enqueue(S.Pop());
                    S.Pop();
                }
                else if(D.ContainsKey(token))
                {
                    while(S.Count>0 && D[token]<=D[S.Peek()])
                    Q.Enqueue(S.Pop());
                    S.Push(token);
                }
                else if(Double.TryParse(token, out tok) || token=="x")Q.Enqueue(token);
            }
            while(S.Count > 0)Q.Enqueue(S.Pop());

            foreach(string t in Q.ToArray())
            {
            L.Add(t);
            Console.Write("{0} ",t);
            }
            Console.WriteLine();
        }

        public double ObliczX()
        {
            double token1;
            Stack<double> So = new Stack<double>();
            foreach(string token in L)
            {
                if(Double.TryParse(token, out token1))
                So.Push(token1);
                else if(token=="x")So.Push(this.zmienna);
                else if(D.ContainsKey(token))
                {
                    double a = So.Pop();
                    if(D[token]==4)
                    {
                        if(token=="abs") a = Math.Abs(a);
                        else if(token=="cos") a = Math.Cos(a);
                        else if(token=="exp") a = Math.Exp(a);
                        else if(token=="log") a = Math.Log(a);
                        else if(token=="sin") a = Math.Sin(a);
                        else if(token=="sqrt") a = Math.Sqrt(a);
                        else if(token=="tan") a = Math.Tan(a);
                        else if(token=="cosh") a = Math.Cosh(a);
                        else if(token=="sinh") a = Math.Sinh(a);
                        else if(token=="tanh") a = Math.Tanh(a);
                        else if(token=="acos") a = Math.Acos(a);
                        else if(token=="asin") a = Math.Asin(a);
                        else if(token=="atan") a = Math.Atan(a);
                    }
                    else
                    {
                        double b = So.Pop();
                        if(token=="+") a += b;
                        else if(token=="-") a = b-a;
                        else if(token=="*") a *= b;
                        else if(token=="/")
                        {
                            if(a==0)
                            {
                                Console.WriteLine("Nie dzielimy przez 0");
                                Environment.Exit(0);
                            }
                            else a = b/a;
                        } 
                        else if(token=="^") a = Math.Pow(b,a);
                    }
                    So.Push(a);
                }
            }
            
            if(this.minus == 1) return So.Pop()*(-1);
            else return So.Pop();
            
        }


        public void ObliczWieleX()
        {
            
            double delta = (this.max-this.min)/(krok-1);
            this.zmienna = this.min;

            for(int i = 0; i < this.krok; i++)
            {
                Console.WriteLine("{0} => {1}", this.zmienna,this.ObliczX());
                
                this.zmienna += delta;
            }
        }
    }
}