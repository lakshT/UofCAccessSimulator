using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace acSimulator
{
    public partial class Form1 : Form
    {
        //Database Connection String
        SqlConnection conn = new SqlConnection("Data Source = LAKSHYA; Initial Catalog = acSimulator; User ID = sa;Password = lakshya@2691");

        //Define tempvariables
        int tempX = 0;
        int tempY = 0;
        int tempX2 = 0;
        int tempY2 = 0;
        int tempX3 = 0;
        int tempY3 = 0;

        //Path arrays
        string[] user1 = new string[] { };
        string[] user2 = new string[] { };
        string[] user3 = new string[] { };

        //Time arrays
        string[] user1t = new string[] { };
        string[] user2t = new string[] { };
        string[] user3t = new string[] { };

        //temp variables for checking path
        string tempU1 = null;
        string tempU2 = null;
        string tempU3 = null;

        //temp variables for checking time
        string tempU1t = null;
        string tempU2t = null;
        string tempU3t = null;

        //Path access counts
        int countU1 = 0;
        int countU2 = 0;
        int countU3 = 0;

        //Time access counts
        int countU1t = 0;
        int countU2t = 0;
        int countU3t = 0;

        //Light controller
        bool turnGreen = false;

        //Define global colors
        Color red = Color.Red;
        Color green = Color.Green;
        Color blue = Color.Blue;
        Color colortoUse = Color.Black;

        //Stop time and wake time
        DateTime stoptimeu1;
        DateTime stoptimeu2;
        DateTime stoptimeu3;
        DateTime waketimeu1;
        DateTime waketimeu2;
        DateTime waketimeu3;

        //Time difference
        int tDiffu1 = 0;
        int tDiffu2 = 0;
        int tDiffu3 = 0;

        public Form1()
        {
            InitializeComponent();
            initPositions();

            stoptimeu1 = Convert.ToDateTime(null);
            stoptimeu2 = Convert.ToDateTime(null);
            stoptimeu3 = Convert.ToDateTime(null);
            waketimeu1 = Convert.ToDateTime(null);
            waketimeu2 = Convert.ToDateTime(null);
            waketimeu3 = Convert.ToDateTime(null);


            string u1 = getAccesslist("User1");
            user1 = u1.Split(':');
            string u2 = getAccesslist("User2");
            user2 = u2.Split(':');
            string u3 = getAccesslist("User3");
            user3 = u3.Split(':');
            u1 = getTimelist("User1");
            user1t = u1.Split(';');
            u2 = getTimelist("User2");
            user2t = u2.Split(';');
            u3 = getTimelist("User3");
            user3t = u3.Split(';');
        }


        string getTimelist(string uname)
        {
            string list = null;
            conn.Open();
            SqlCommand cmd = new SqlCommand("select utime from usertime as t,userpath as p where p.uid = t.uid and p.uname = '"+ uname +"'", conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                list = rdr[0].ToString();
            }
            conn.Close();
            return list;
        }

        string getAccesslist(string uname)
        {
            string list = null;
            conn.Open();
            SqlCommand cmd = new SqlCommand("Select upath from userpath where uname = '" + uname + "'", conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            if(rdr.Read())
            {
                list = rdr[0].ToString();
            }
            conn.Close();
            return list;
        }

        bool validateTime(string uname)
        {
            if (uname == "User1")
            {
                DateTime dt = new DateTime();
                dt = Convert.ToDateTime(user1t[countU1t]);

                if (DateTime.Now.AddSeconds(-60) <= dt.AddSeconds(tDiffu1) && DateTime.Now.AddSeconds(60) >= dt.AddSeconds(tDiffu1))
                {
                    countU1t = countU1t + 1;
                    return true;
                }
                else
                {
                    //countU1 = countU1 - 1; 
                    return false;
                }
            }
            if (uname == "User2")
            {
                DateTime dt = new DateTime();
                dt = Convert.ToDateTime(user2t[countU2t]);

                if (DateTime.Now.AddSeconds(-60) <= dt.AddSeconds(tDiffu2) && DateTime.Now.AddSeconds(60) >= dt.AddSeconds(tDiffu2))
                {
                    countU2t = countU2t + 1;
                    return true;
                }
                else
                {
                    //countU2 = countU2 - 1;
                    return false;
                }
            }
            if (uname == "User3")
            {
                DateTime dt = new DateTime();
                dt = Convert.ToDateTime(user3t[countU3t]);

                if (DateTime.Now.AddSeconds(-60) <= dt.AddSeconds(tDiffu3) && DateTime.Now.AddSeconds(60) >= dt.AddSeconds(tDiffu3))
                {
                    countU3t = countU3t + 1;
                    return true;
                }
                else
                {
                    //countU3 = countU3 - 1;
                    return false;
                }
            }
            return false;
        }

        bool validataPath(string uname, TextBox tx)
        {
            if(uname == "User1")
            {
                if(countU1 == 0 && user1[0] == tx.Text.ToUpper())
                {
                    //validate time
                    bool checkTime = validateTime("User1");
                    if (checkTime == false)
                    {
                        MessageBox.Show("This access point is not available to this user due to time expiry");
                        return false;
                    }

                    tempU1 = tx.Text.ToUpper();
                    countU1 = countU1 + 1; 
                    return true;
                }
                else if ((countU1 > 0 && countU1 < user1.Length) && (tempU1 == user1[countU1 - 1] && user1[countU1] == tx.Text.ToUpper()))
                {
                    //validate time
                    bool checkTime = validateTime("User1");
                    if (checkTime == false)
                    {
                        MessageBox.Show("This access point is not available to this user due to time expiry");
                        return false;
                    }

                    tempU1 = tx.Text.ToUpper();
                    countU1 = countU1 + 1;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            if(uname == "User2")
            {
                if (countU2 == 0 && user2[0] == tx.Text.ToUpper())
                {
                    //validate time
                    bool checkTime = validateTime("User2");
                    if (checkTime == false)
                    {
                        MessageBox.Show("This access point is not available to this user due to time expiry");
                        return false;
                    }

                    tempU2 = tx.Text.ToUpper();
                    countU2 = countU2 + 1;
                    return true;
                }
                else if ((countU2 > 0 && countU2 < user2.Length) && (tempU2 == user2[countU2 - 1] && user2[countU2] == tx.Text.ToUpper()))
                {
                    //validate time
                    bool checkTime = validateTime("User2");
                    if (checkTime == false)
                    {
                        MessageBox.Show("This access point is not available to this user due to time expiry");
                        return false;
                    }

                    tempU2 = tx.Text.ToUpper();
                    countU2 = countU2 + 1;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            if (uname == "User3")
            {
                if (countU3 == 0 && user3[0] == tx.Text.ToUpper())
                {
                    //validate time
                    bool checkTime = validateTime("User3");
                    if (checkTime == false)
                    {
                        MessageBox.Show("This access point is not available to this user due to time expiry");
                        return false;
                    }

                    tempU3 = tx.Text.ToUpper();
                    countU3 = countU3 + 1;
                    return true;
                }
                else if ((countU3 > 0 && countU3 < user3.Length) && (tempU3 == user3[countU3 - 1] && user3[countU3] == tx.Text.ToUpper()))
                {
                    //validate time
                    bool checkTime = validateTime("User3");
                    if (checkTime == false)
                    {
                        MessageBox.Show("This access point is not available to this user due to time expiry");
                        return false;
                    }

                    tempU3 = tx.Text.ToUpper();
                    countU3 = countU3 + 1;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        void drawLine(int x1, int y1, int x2, int y2, Color colorname)
        {
            Graphics g = this.CreateGraphics();
            Pen pen = new Pen(colorname, 3);
            Point point1 = new Point(x1, y1);
            Point point2 = new Point(x2, y2);
            g.DrawLine(pen, point1, point2);
        }

        void initPositions()
        {
            as1.Location = new Point(160, 60);
            l1.Location = new Point(134, 60);
            as2.Location = new Point(325, 190);
            l2.Location = new Point(299, 190);
            as3.Location = new Point(500, 121);
            l3.Location = new Point(474, 121);
            as4.Location = new Point(900, 65);
            l4.Location = new Point(874, 66);
            as5.Location = new Point(1150, 165);
            l5.Location = new Point(1124, 165);
            as6.Location = new Point(425, 335);
            l6.Location = new Point(399, 335);
            as7.Location = new Point(820, 450);
            l7.Location = new Point(794, 450);
            as8.Location = new Point(790, 600);
            l8.Location = new Point(764, 600);
            as9.Location = new Point(550, 600);
            l9.Location = new Point(524, 600);
            as10.Location = new Point(245, 545);
            l10.Location = new Point(213, 545);
            as11.Location = new Point(745, 245);
            l11.Location = new Point(713, 245);
            as12.Location = new Point(650, 450);
            l12.Location = new Point(618, 450);
            groupBox1.Location = new Point(1000, 500);
        }

        

        void screenPositionFunc(int x, int y, Color colortoUsehere, TextBox tx)
        {
            turnGreen = false;
            if (tx == textBox1)
            {
                //validataPath for user1

                bool checkPath= validataPath("User1", tx);
                if(checkPath== false)
                {
                    MessageBox.Show("This access point is not available to this user.");
                    return;
                }

                ////validate time
                //bool checkTime = validateTime("User1");
                //if (checkTime == false)
                //{
                //    MessageBox.Show("This access point is not available to this user due to time expiry");
                //    return;
                //}


                if (tempX != 0 && tempY != 0)
                {
                    //draw a line
                    drawLine(tempX, tempY, x, y, colortoUsehere);
                    //store current x and y in temp x and y
                    tempX = x;
                    tempY = y;
                    turnGreen = true;
                }
                else
                {
                    turnGreen = true;
                    tempX = x;
                    tempY = y;
                }
            }
            if (tx == textBox2)
            {
                //validataPath for user2
                bool checkPath = validataPath("User2", tx);
                if (checkPath == false)
                {
                    MessageBox.Show("This access point is not available to this user.");
                    return;
                }

                ////validate time
                //bool checkTime = validateTime("User2");
                //if (checkTime == false)
                //{
                //    MessageBox.Show("This access point is not available to this user due to time expiry");
                //    return;
                //}

                y = y + 5;
                if (tempX2 != 0 && tempY2 != 0)
                {
                    //draw a line
                    drawLine(tempX2, tempY2, x, y, colortoUsehere);
                    //store current x and y in temp x and y
                    tempX2 = x;
                    tempY2 = y;
                    turnGreen = true;
                }
                else
                {
                    turnGreen = true;
                    tempX2 = x;
                    tempY2 = y;
                }
            }
            if(tx == textBox3)
            {
                //validataPath for user3
                bool checkPath= validataPath("User3", tx);
                if (checkPath== false)
                {
                    MessageBox.Show("This access point is not available to this user.");
                    return;
                }

                ////validate time
                //bool checkTime = validateTime("User3");
                //if (checkTime == false)
                //{
                //    MessageBox.Show("This access point is not available to this user due to time expiry");
                //    return;
                //}


                y = y + 10;
                if (tempX3 != 0 && tempY3 != 0)
                {
                    //draw a line
                    drawLine(tempX3, tempY3, x, y, colortoUsehere);
                    //store current x and y in temp x and y
                    tempX3 = x;
                    tempY3 = y;
                    turnGreen = true;
                }
                else
                {
                    turnGreen = true;
                    tempX3 = x;
                    tempY3 = y;
                }
            }
        }

        void handleinput(TextBox tx)
        {
            if(tx == textBox1 && tx.Text.ToUpper() == "SLEEP")
            {
                stoptimeu1 = Convert.ToDateTime(null);
                waketimeu1 = Convert.ToDateTime(null);
            }
            if(tx == textBox2 && tx.Text.ToUpper() == "SLEEP")
            {
                stoptimeu2 = Convert.ToDateTime(null);
                waketimeu2 = Convert.ToDateTime(null);
                
            }
            if (tx == textBox3 && tx.Text.ToUpper() == "SLEEP")
            {
                stoptimeu3 = Convert.ToDateTime(null);
                waketimeu3 = Convert.ToDateTime(null);
            }

            if (tx == textBox1)
            {
                colortoUse = red;
            }
            if (tx == textBox2)
            {
                colortoUse = green;
            }
            if (tx == textBox3)
            {
                colortoUse = blue;
            }
            if (tx.Text.ToUpper() == "AS1")
            {
                //validataPath function form database
                //if validataPathd then continue
                //else give invalid message and return
                var screenPosition = as1.Location;
                //
                int x2 = screenPosition.X;
                int y2 = screenPosition.Y;
                //Call function
                screenPositionFunc(x2, y2, colortoUse,tx);
                if(turnGreen == true)
                {
                    as1.BackgroundImage = Properties.Resources.greendot;
                }
                
            }
            if (tx.Text.ToUpper() == "AS2")
            {
                var screenPosition = as2.Location;
                //
                int x2 = screenPosition.X;
                int y2 = screenPosition.Y;
                //Call function
                screenPositionFunc(x2, y2, colortoUse,tx);
                if (turnGreen == true)
                {
                    as2.BackgroundImage = Properties.Resources.greendot;
                }
            }
            if (tx.Text.ToUpper() == "AS3")
            {
                var screenPosition = as3.Location;
                //
                int x2 = screenPosition.X;
                int y2 = screenPosition.Y;
                //Call function
                screenPositionFunc(x2, y2, colortoUse, tx);
                if (turnGreen == true)
                {
                    as3.BackgroundImage = Properties.Resources.greendot;
                }
            }
            if (tx.Text.ToUpper() == "AS4")
            {
                var screenPosition = as4.Location;
                //
                int x2 = screenPosition.X;
                int y2 = screenPosition.Y;
                //Call function
                screenPositionFunc(x2, y2, colortoUse, tx);
                if (turnGreen == true)
                {
                    as4.BackgroundImage = Properties.Resources.greendot;
                }
            }
            if (tx.Text.ToUpper() == "AS5")
            {
                var screenPosition = as5.Location;
                //
                int x2 = screenPosition.X;
                int y2 = screenPosition.Y;
                //Call function
                screenPositionFunc(x2, y2, colortoUse, tx);
                if (turnGreen == true)
                {
                    as5.BackgroundImage = Properties.Resources.greendot;
                }
            }
            if (tx.Text.ToUpper() == "AS6")
            {
                var screenPosition = as6.Location;
                //
                int x2 = screenPosition.X;
                int y2 = screenPosition.Y;
                //Call function
                screenPositionFunc(x2, y2, colortoUse, tx);
                if (turnGreen == true)
                {
                    as6.BackgroundImage = Properties.Resources.greendot;
                }
            }
            if (tx.Text.ToUpper() == "AS7")
            {
                var screenPosition = as7.Location;
                //
                int x2 = screenPosition.X;
                int y2 = screenPosition.Y;
                //Call function
                screenPositionFunc(x2, y2, colortoUse, tx);
                if (turnGreen == true)
                {
                    as7.BackgroundImage = Properties.Resources.greendot;
                }
            }
            if (tx.Text.ToUpper() == "AS8")
            {
                var screenPosition = as8.Location;
                //
                int x2 = screenPosition.X;
                int y2 = screenPosition.Y;
                //Call function
                screenPositionFunc(x2, y2, colortoUse, tx);
                if (turnGreen == true)
                {
                    as8.BackgroundImage = Properties.Resources.greendot;
                }
            }
            if (tx.Text.ToUpper() == "AS9")
            {
                var screenPosition = as9.Location;
                //
                int x2 = screenPosition.X;
                int y2 = screenPosition.Y;
                //Call function
                screenPositionFunc(x2, y2, colortoUse, tx);
                if (turnGreen == true)
                {
                    as9.BackgroundImage = Properties.Resources.greendot;
                }
            }
            if (tx.Text.ToUpper() == "AS10")
            {
                var screenPosition = as10.Location;
                //
                int x2 = screenPosition.X;
                int y2 = screenPosition.Y;
                //Call function
                screenPositionFunc(x2, y2, colortoUse, tx);
                if (turnGreen == true)
                {
                    as10.BackgroundImage = Properties.Resources.greendot;
                }
            }
            if (tx.Text.ToUpper() == "AS11")
            {
                var screenPosition = as11.Location;
                //
                int x2 = screenPosition.X;
                int y2 = screenPosition.Y;
                //Call function
                screenPositionFunc(x2, y2, colortoUse, tx);
                if (turnGreen == true)
                {
                    as11.BackgroundImage = Properties.Resources.greendot;
                }
            }
            if (tx.Text.ToUpper() == "AS12")
            {
                var screenPosition = as12.Location;
                //
                int x2 = screenPosition.X;
                int y2 = screenPosition.Y;
                //Call function
                screenPositionFunc(x2, y2, colortoUse, tx);
                if (turnGreen == true)
                {
                    as12.BackgroundImage = Properties.Resources.greendot;
                }
            }
            if (tx.Text.ToUpper() == "SLEEP")
            {
                if(tx == textBox1)
                {
                    label5.Visible = true;
                    stoptimeu1 = DateTime.Now;
                }
                if (tx == textBox2)
                {
                    label6.Visible = true;
                    stoptimeu2 = DateTime.Now;
                }
                if (tx == textBox3)
                {
                    label7.Visible = true;
                    stoptimeu3 = DateTime.Now;
                }
            }
            if (tx.Text.ToUpper() == "WAKE")
            {
                if (tx == textBox1)
                {
                    label5.Visible = false;
                    waketimeu1 = DateTime.Now;
                    TimeSpan ts = waketimeu1.Subtract(stoptimeu1);
                    tDiffu1 = tDiffu1 + timespanTime(ts);
                }
                if (tx == textBox2)
                {
                    label6.Visible = false;
                    waketimeu2 = DateTime.Now;
                    TimeSpan ts = waketimeu2.Subtract(stoptimeu2);
                    tDiffu2 = tDiffu2 + timespanTime(ts);
                }
                if (tx == textBox3)
                {
                    label7.Visible = false;
                    waketimeu3 = DateTime.Now;
                    TimeSpan ts = waketimeu3.Subtract(stoptimeu3);
                    tDiffu3 = tDiffu3 + timespanTime(ts);
                }
            }
        }

        int timespanTime(TimeSpan ts)
        {
            int hours = ts.Hours;
            int minutes = ts.Minutes;
            int seconds = ts.Seconds;

            int totalsec = hours * 3600 + minutes * 60 + seconds;
            return totalsec;
        }


        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                handleinput(textBox1);
                textBox1.Text = string.Empty;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                handleinput(textBox2);
                textBox2.Text = string.Empty;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                handleinput(textBox3);
                textBox3.Text = string.Empty;
            }
        }
    }
}
