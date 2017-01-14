using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;

namespace YazLab2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            
        }
       
        SqlConnection bag = new SqlConnection(@"Data Source=.\;Initial Catalog=Test;Integrated Security=True");
        int count, Aranan,id;
        int tavan,ab,sayac=0,bilinen;
        int[] dizi = new int[1000];
        int t1son, tsonbas, tsonson, t2bas, t2son;

        Thread th1, th2, th3,th4;
        private void button1_Click(object sender, EventArgs e)
        {
            
            backgroundWorker1.RunWorkerAsync();
            
        }

        public void Thread1()
        {
            for (int a = 1; a <= t1son; a++)
            {
                if (sayac != 0 ) th1.Abort();

                if (Aranan % dizi[a] == 0) {
                    label16.Text = "true";
                    sayac++;
                    bilinen=dizi[a];
                    th1.Abort();
                    
                    
                
                }

            }

        }
        public void Threadson()
        {


            for (int a = tsonbas; a > tsonson; a--)
            {

                if (sayac != 0) th2.Abort();


                if (Aranan % dizi[a] == 0) {

                    bilinen = dizi[a];
                    sayac++;
                    if (ab >= 200)
                    {

                        label18.Text = "true";
                    }
                    else label17.Text = "true";

                    th2.Abort();
                   
                    

                }

            }





        }

        public void Thread2()
        {
            for (int a = t2bas; a <= t2son; a++)
            {
                if (sayac != 0) th4.Abort();

                if (Aranan % dizi[a] == 0)
                {
                    bilinen = dizi[a];
                    sayac++;
                    label17.Text = "true";
                    th4.Abort();
                    
                }

            }


        }

        public void thread3() {

           

            
            SqlConnection bag1 = new SqlConnection(@"Data Source=.\;Initial Catalog=Test;Integrated Security=True");
       
            bag1.Open();

            id++;
            SqlCommand kmt1 = new SqlCommand("insert into bbbb (id,sayi) values ('" + id + "','" + Aranan + "')", bag1);
            kmt1.ExecuteNonQuery();



            bag1.Close();

            th3.Abort();
        
        
        
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            for (; ; )
            {
                

                bag.Open();
                SqlCommand komut = new SqlCommand("select count(id) from bbbb", bag);
                //veri tabanında kaç satır oldugunu buluyoruz
                object aa = komut.ExecuteScalar();
                count = Convert.ToInt32(aa);
                id = count;
                SqlCommand kmt = new SqlCommand("select sayi from bbbb where id=" + count + "", bag);
                //veritabanındaki son asal sayı
                object bb = kmt.ExecuteScalar();
                Aranan = Convert.ToInt32(bb);
                bag.Close();


                for (int k = 0; k < 50; k++)
                {

                    Aranan += 2;
                    
                    label1.Text = Aranan.ToString();
                  //  textBox1.Text = th1.IsAlive.ToString();

                    tavan = Convert.ToInt32(Math.Sqrt(Aranan));

                    bag.Open();

                    SqlCommand aradaki = new SqlCommand("select count(sayi) from bbbb where sayi<=" + tavan + "", bag);
                    object dd = aradaki.ExecuteScalar();
                    ab = Convert.ToInt32(dd);
                    // ab=2 ilk çalıştırıldığında
                    for (int a = 1; a <= ab; a++)
                    {

                        SqlCommand sayi = new SqlCommand("select sayi from bbbb where id=" + a + "", bag);
                        object ac = sayi.ExecuteScalar();
                        dizi[a] = Convert.ToInt32(ac);



                    }
                    bag.Close();


                    if (ab < 100)
                    {
                        t1son = ab / 2;
                        tsonbas = ab;
                        tsonson = ab / 2;
                        label10.Text = "2";
                        label11.Text = dizi[t1son].ToString();
                        label12.Text = dizi[tsonbas].ToString();
                        label13.Text = dizi[tsonson].ToString();
                        th1 = new Thread(Thread1);
                        th2 = new Thread(Threadson);
                      //  th1.IsBackground = true;
                       // th2.IsBackground = true;
                        th1.Start();
                        th2.Start();

                    }
                    else
                    {
                        int key;
                        key = (ab / 100) + 1;
                        if (key == 2)
                        {
                            t1son = ab / 2;
                            tsonbas = ab;
                            tsonson = ab / 2;
                            label10.Text = "2";
                            label11.Text = dizi[t1son].ToString();
                            label12.Text = dizi[tsonbas].ToString();
                            label13.Text = dizi[tsonson].ToString();
                            th1 = new Thread(Thread1);
                            th2 = new Thread(Threadson);
                         //   th1.IsBackground = true;
                         //   th2.IsBackground = true;
                            th1.Start();
                            th2.Start();

                        }
                        else
                        {

                            t1son = ab / 3;
                            t2bas = ab / 3 + 1;
                            t2son = (2 * ab) / 3;
                            tsonbas = ab;
                            tsonson = (2 * ab) / 3;
                            label10.Text = "2";
                            label11.Text = dizi[t1son].ToString();
                            label14.Text = dizi[tsonbas].ToString();
                            label15.Text = dizi[tsonson].ToString();
                            label12.Text = dizi[t2bas].ToString();
                            label13.Text = dizi[t2son].ToString();
                            th1 = new Thread(Thread1);
                            th2 = new Thread(Threadson);
                            th4 = new Thread(Thread2);
                         //   th1.IsBackground = true;
                         //   th2.IsBackground = true;
                         //   th4.IsBackground = true;
                            th1.Start();
                            th2.Start();
                            th4.Start();



                        }


                    }



               if (th1.IsAlive == true || th2.IsAlive == true) Thread.Sleep(500);

                    if (sayac == 0)
                    {
                        label16.Text = "false";
                        if (ab >= 200)
                        {
                            label16.Text = "false";
                            label17.Text = "false";

                        }
                        else label17.Text = "false";


                        th3 = new Thread(thread3);
                        th3.Start();

                        break;


                    }
                    else
                    {
                        label2.Text = Aranan.ToString()+" sayısı "+bilinen.ToString()+" bölündüğü\n için asal değildir!";

                    }


                    sayac = 0;



                }
                
                
               if (th3.IsAlive == true ) Thread.Sleep(500);
               
             
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
           
 
        }

        


    }
}
