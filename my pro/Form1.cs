using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
using System.Threading;
using System.Media;
using NAudio.Wave;
using Timer = System.Windows.Forms.Timer; // تعیین پیش‌فرض برای Timer
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net.Http.Headers;


namespace my_pro
{
    public partial class Form1 : Form
    {
        Dictionary<int, double> dataDict4 = new Dictionary<int, double>();
        Dictionary<int, double> dataDict5 = new Dictionary<int, double>();

        string ps4 = "ps4.txt";
        string ps5 = "ps5.txt";











        public Form1()
        {
            InitializeComponent();

            // تنظیم تایمر
            Timer timer = new Timer();
            timer.Interval = 60000; // 5 دقیقه (300000 میلی‌ثانیه)
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();

        }



        private static readonly HttpClient client = new HttpClient();


        private void Timer_Tick(object sender, EventArgs e)
        {
            SaveDataGridViewToTextFile(dgv_ps5, "PS5BACKUP.txt");
            SaveDataGridViewToTextFile(dgv_ps4, "PS4BACKUP.txt");



            // 2. ارسال فایل به Worker بعد از ذخیره‌سازی
            Task.Run(() => SendFileContentToWorker("PS5BACKUP.txt"));

            Task.Run(() => SendFileContentToWorker("PS4BACKUP.txt"));

        }


        private async Task SendFileContentToWorker(string filePath)
        {
            string workerUrl = "https://snowy-art-a1b3.aliborhanian117.workers.dev/"; // آدرس Worker خودت

            try
            {
                string fileContent = File.ReadAllText(filePath); // محتوای فایل رو می‌خونه
                string fileName = Path.GetFileName(filePath); // نام فایل رو جدا می‌کنه

                using (HttpClient client = new HttpClient())
                {
                    var values = new Dictionary<string, string>
            {
                { "filename", fileName }, // ارسال نام فایل
                { "content", fileContent } // ارسال محتوای فایل
            };

                    var content = new FormUrlEncodedContent(values);
                    HttpResponseMessage response = await client.PostAsync(workerUrl, content);
                    string responseString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Response from Worker: " + responseString);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("خطا در ارسال محتوا: " + ex.Message);
            }
        }





        private void SaveDataGridViewToTextFile(DataGridView dgv, string filePath)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    // ذخیره هدرهای ستون‌ها
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        sw.Write(dgv.Columns[i].HeaderText);
                        if (i < dgv.Columns.Count - 1)
                        {
                            sw.Write("| \t |"); // جداکننده تب (Tab)
                        }
                    }
                    sw.WriteLine();

                    // ذخیره داده‌های هر سطر
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        for (int i = 0; i < dgv.Columns.Count; i++)
                        {
                            if (row.Cells[i].Value != null)
                            {
                                sw.Write(row.Cells[i].Value.ToString());
                            }
                            if (i < dgv.Columns.Count - 1)
                            {
                                sw.Write("|\t\t|"); // جداکننده تب (Tab)
                            }
                        }
                        sw.WriteLine();
                    }
                }

                //MessageBo.Show("داده‌ها با موفقیت ذخیره شدند.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در ذخیره فایل: " + ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void addcostviwe()
        {
            label16.Text = ((dataDict5[1] * 60) * 60).ToString();
            label13.Text = ((dataDict5[2] * 60) * 60).ToString();
            label15.Text = ((dataDict5[3] * 60) * 60).ToString();
            label14.Text = ((dataDict5[4] * 60) * 60).ToString();


            label18.Text = ((dataDict4[1] * 60) * 60).ToString();
            label20.Text = ((dataDict4[2] * 60) * 60).ToString();
            label17.Text = ((dataDict4[3] * 60) * 60).ToString();
            label19.Text = ((dataDict4[4] * 60) * 60).ToString();
        }















        public string sysnumcombovalue;
        public string radcontrollervalue;
        public int costtimevalue;
        public int costmoneyvalue;
        public bool costfreevalue;
        public int costadditionaltimevalue5;
        public int costadditionaltimevalue4;
        DateTime Datetime = DateTime.Now;

        //Functions

        private void AddSyscomboItem()
        {
            for (int i = 0; i <= 20; i++)
            {
                if (i == 0)
                {
                    sysnum_combo5.Items.Add("انتخاب کنید");
                    sysnum_combo5.Text = "انتخاب کنید";

                    sysnum_combo4.Items.Add("انتخاب کنید");
                    sysnum_combo4.Text = "انتخاب کنید";
                }
                else
                {
                    sysnum_combo5.Items.Add(i);
                    sysnum_combo4.Items.Add(i);
                }
            }


            for (int i = 0; i <= 4; i++)
            {
                //combo_setcost_type.Text = "انتخاب کنید";
                //combo_setcost_sysnum.Text = "انتخاب کنید";

                if (i == 0)
                {
                    combo_setcost_sysnum.Items.Add("انتخاب کنید");
                    combo_setcost_sysnum.Text = "انتخاب کنید";
                    combo_setcost_type.Items.Add("انتخاب کنید");
                    combo_setcost_type.Text = "انتخاب کنید";
                    combo_setcost_type.Items.Add("ps5");
                    combo_setcost_type.Items.Add("ps4");
                }
                else
                {
                    combo_setcost_sysnum.Items.Add(i);
                }
            }



        }

        private void CheckSelectedItem_ComboSysNum5()
        {
            if (sysnum_combo5.SelectedItem.ToString() != "انتخاب کنید")
            {
                controller_grp5.Enabled = true;
                sysnumcombovalue = sysnum_combo5.SelectedItem.ToString();
                
            }
            else
            {
                sysnumcombovalue = sysnum_combo5.SelectedItem.ToString();
                controller_grp5.Enabled = false;
                cost_grp5.Enabled = false;
            }

        }


        private void CheckSelected_RadController5()
        {
            foreach (Control control in controller_grp5.Controls)
            {
                if (control is RadioButton radioButton && radioButton.Checked)
                {
                    radcontrollervalue = radioButton.Text;
                    break; // Exit the loop after finding the checked radio button
                }
            }
        }

        private void CheckSelected_RadCost5()
        {
            foreach (Control control in cost_grp5.Controls)
            {
                if (control is RadioButton radioButton && radioButton.Checked)
                {
                    switch (radioButton.Name)
                    {
                        case "radtime_cost5":
                            if (txtbxtime_cost5.Text.Length > 0)
                            {
                                costtimevalue = Convert.ToInt32(txtbxtime_cost5.Text);
                                costadditionaltimevalue5 = (txtbxadditionaltime_cost5.Text.Length > 0 ? int.Parse(txtbxadditionaltime_cost5.Text) : 0);
                                return;
                            }
                            break;
                        case "radmoney_cost5":
                            if (txtbxmoney_cost5.Text.Length > 0)
                            {
                                // دریافت مبلغ ورودی
                                double moneyCost = Convert.ToDouble(txtbxmoney_cost5.Text);

                                // دریافت نرخ هر دقیقه برای تعداد جوی‌استیک‌های انتخاب‌شده
                                int radCategory = GetCategoryFromRadController5();
                                double ratePerMinute = dataDict5[radCategory];

                                // محاسبه زمان بر اساس مبلغ ورودی
                                costtimevalue = (int)((moneyCost / ratePerMinute)) / 60;

                                // ذخیره مبلغ ورودی
                                costmoneyvalue = (int)moneyCost;

                                // زمان اضافی (اگر وجود دارد)
                                costadditionaltimevalue5 = (txtbxadditionaltime_cost5.Text.Length > 0 ? int.Parse(txtbxadditionaltime_cost5.Text) : 0);
                                return;
                            }
                            break;
                        case "radfree_cost5":
                            costfreevalue = true;
                            costtimevalue = 0; // زمان آزاد
                            costadditionaltimevalue5 = (txtbxadditionaltime_cost5.Text.Length > 0 ? int.Parse(txtbxadditionaltime_cost5.Text) : 0);
                            return;
                    }
                }
            }
        }

        // Helper method to get the category (number of joysticks) from the selected radio button
        private int GetCategoryFromRadController5()
        {
            var categoryValues = new Dictionary<string, int>
    {
        { "تک دسته", 1 },
        { "دو دسته", 2 },
        { "سه دسته", 3 },
        { "چهار دسته", 4 }
    };

            return categoryValues[radcontrollervalue];
        }


        // دیکشنری تایمرها، زمان باقی‌مانده و زمان اولیه
        private Dictionary<int, System.Threading.Timer> timers5 = new Dictionary<int, System.Threading.Timer>();
        private Dictionary<int, int> remainingTimes5 = new Dictionary<int, int>();
        private Dictionary<int, int> extraTimes5 = new Dictionary<int, int>(); // زمان اضافه‌شده پس از رسیدن به صفر
        private Dictionary<int, int> initialTimes5 = new Dictionary<int, int>();
        private Dictionary<int, bool> isTimeExpired5 = new Dictionary<int, bool>(); // وضعیت اتمام زمان

        // متد برای افزودن ردیف جدید
        private void Add_datarow5()
        {
            var categoryValues = new Dictionary<string, int>
    {
        { "تک دسته", 1 },
        { "دو دسته", 2 },
        { "سه دسته", 3 },
        { "چهار دسته", 4 }
    };

            if (categoryValues.TryGetValue(radcontrollervalue, out int radcontrollervalueASnum))
            {
                // بررسی تکراری نبودن شماره سیستم
                foreach (DataGridViewRow row in dgv_ps5.Rows)
                {
                    if (row.Cells[0].Value.ToString() == sysnumcombovalue.ToString())
                    {
                        MessageBox.Show("این شماره سیستم قبلاً وارد شده است.");
                        return;
                    }
                }

                // محاسبه زمان کل بر اساس دقیقه و ثانیه
                int totalTimeInMinutes = costtimevalue;
                int totalTimeInSeconds = totalTimeInMinutes * 60;

                // اگر زمان آزاد است
                if (costfreevalue)
                {
                    totalTimeInMinutes = 0; // زمان آزاد
                    totalTimeInSeconds = 0;
                }

                // کم کردن زمان اضافه از زمان باقی‌مانده
                if (costadditionaltimevalue5 > 0)
                {
                    int extraTimeInSeconds = costadditionaltimevalue5 * 60;
                    totalTimeInSeconds -= extraTimeInSeconds;

                    // اطمینان از اینکه زمان باقی‌مانده منفی نشود
                    if (totalTimeInSeconds < 0)
                    {
                        totalTimeInSeconds = 0;
                    }
                }

                // محاسبه هزینه بر اساس نرخ هر ثانیه
                double ratePerSecond = dataDict5[radcontrollervalueASnum];
                double costPerSystem = (ratePerSecond * totalTimeInMinutes) * 60;
                costPerSystem = Math.Round(costPerSystem, 15);

                // افزودن ردیف به DataGridView
                int rowIndex;
                if (costadditionaltimevalue5 == 0)
                {
                    rowIndex = dgv_ps5.Rows.Add(
                        sysnumcombovalue,
                        radcontrollervalue,
                        costfreevalue ? "آزاد" : $"{totalTimeInMinutes} دقیقه", // زمان کل
                        DateTime.Now.ToString("H:m:s"), // زمان شروع
                        "0", // مبلغ اولیه
                        totalTimeInSeconds.ToString(),
                        "0" // زمان باقی‌مانده
                        ,
                        "0"
                        ,
                        "0"
                    );
                }
                else
                {
                    TimeSpan time = TimeSpan.FromMinutes(costadditionaltimevalue5);
                    DateTime sttime = DateTime.Now - time;

                    rowIndex = dgv_ps5.Rows.Add(
                        sysnumcombovalue,
                        radcontrollervalue,
                        costfreevalue ? "آزاد" : $"{totalTimeInMinutes} دقیقه", // زمان کل
                        sttime.ToString("H:m:s"), // زمان شروع
                        "0", // مبلغ اولیه
                        totalTimeInSeconds.ToString(),
                        "0" // زمان باقی‌مانده
                                                ,
                        "0"
                        ,
                        "0"
                    );
                }

                // ذخیره اطلاعات در دیکشنری‌ها
                remainingTimes5[rowIndex] = totalTimeInSeconds;
                initialTimes5[rowIndex] = totalTimeInSeconds;
                extraTimes5[rowIndex] = 0; // مقدار اضافه‌شده از صفر شروع شود
                isTimeExpired5[rowIndex] = false;

                // ایجاد و شروع تایمر
                System.Threading.Timer timer = new System.Threading.Timer(UpdateTimeRemaining5, rowIndex, 1000, 1000);
                timers5[rowIndex] = timer;
            }
            else
            {
                MessageBox.Show("دسته انتخابی نامعتبر است.");
            }
        }

        // متد برای به‌روزرسانی زمان باقی‌مانده
        private void UpdateTimeRemaining5(object state)
        {
            int rowIndex = (int)state;

            // بررسی معتبر بودن ردیف
            if (rowIndex >= dgv_ps5.Rows.Count || dgv_ps5.Rows[rowIndex].Cells["remnant"] == null)
            {
                Console.WriteLine($"Invalid rowIndex: {rowIndex}");
                return;
            }

            if (!remainingTimes5.ContainsKey(rowIndex)) return;

            if (remainingTimes5[rowIndex] > 0)
            {
                remainingTimes5[rowIndex]--;

                // محاسبه مبلغ بر اساس زمان باقی‌مانده
                double ratePerSecond = dataDict5[GetCategoryFromRowIndex5(rowIndex)];
                double costPerSystem = ratePerSecond * (initialTimes5[rowIndex] - remainingTimes5[rowIndex]);
                costPerSystem = Math.Round(costPerSystem, 15);



                // به‌روزرسانی مبلغ در DataGridView


                // تبدیل مقدار فعلی سلول به عدد
                double currentValue;
                if (double.TryParse(dgv_ps5.Rows[rowIndex].Cells["moneycost"].Value?.ToString(), out currentValue))
                {
                    // انجام محاسبه
                    double newValue = currentValue + ratePerSecond;

                    // ارسال مقدار جدید به سلول با فرمت دو رقم اعشار
                    dgv_ps5.Rows[rowIndex].Cells["moneycost"].Value = newValue.ToString("F2");
                }
                else
                {
                    // اگر مقدار فعلی سلول قابل تبدیل به عدد نباشد، می‌توانید یک مقدار پیش‌فرض قرار دهید یا خطا مدیریت کنید
                    dgv_ps5.Rows[rowIndex].Cells["moneycost"].Value = costPerSystem.ToString("F2");
                }



                

                // به‌روزرسانی زمان باقی‌مانده
                dgv_ps5.Rows[rowIndex].Cells["remnant"].Value = TimeSpan.FromSeconds(remainingTimes5[rowIndex]).ToString(@"hh\:mm\:ss");

                if (remainingTimes5[rowIndex] == 0)
                {
                    isTimeExpired5[rowIndex] = true;
                    PlaySound();





                    string pattern = @"\d+(\.\d+)?"; // الگو برای تشخیص اعداد (اعشاری یا صحیح)

                    Match match = Regex.Match(dgv_ps5.Rows[rowIndex].Cells["wholetime"].Value.ToString(), pattern);

                    if (match.Success)
                    {
                        // اگر عدد پیدا شد، آن را به یک متغیر منتقل کنید
                        string numberString = match.Value;
                        int number = int.Parse(numberString); // تبدیل به عدد
                        number *= 60;
                        dgv_ps5.Rows[rowIndex].Cells["moneycost"].Value = (ratePerSecond*number).ToString("F2");

                    }
                    else
                    {
                        
                    }


                }
            }
            else
            {
                extraTimes5[rowIndex]++;

                // محاسبه مبلغ اضافی
                double extraMoney = CalculateExtraMoney5(rowIndex);

                // به‌روزرسانی مبلغ اضافی
                dgv_ps5.Rows[rowIndex].Cells["extramoney"].Value = extraMoney.ToString("F2");

                // محاسبه مبلغ نهایی (moneycost + extramoney)
                double moneyCost = double.Parse(dgv_ps5.Rows[rowIndex].Cells["moneycost"].Value.ToString());
                double finalMoney = moneyCost + extraMoney;

                // به‌روزرسانی مبلغ نهایی
                dgv_ps5.Rows[rowIndex].Cells["finalmoney"].Value = finalMoney.ToString("F2");

                // به‌روزرسانی زمان اضافی
                dgv_ps5.Rows[rowIndex].Cells["remnant"].Value = "+" + TimeSpan.FromSeconds(extraTimes5[rowIndex]).ToString(@"hh\:mm\:ss");
            }

            // ادامه تایمر
            timers5[rowIndex].Change(1000, 1000);
        }



        // متد برای محاسبه مبلغ اضافی
        private double CalculateExtraMoney5(int rowIndex)
        {
            int extraSeconds = extraTimes5[rowIndex];
            int radCategory = GetCategoryFromRowIndex5(rowIndex);
            double ratePerSecond = dataDict5[radCategory];

            double extraMoney = ratePerSecond * extraSeconds;
            return extraMoney;
        }

        // دریافت دسته از ردیف
        private int GetCategoryFromRowIndex5(int rowIndex)
        {
            string category = dgv_ps5.Rows[rowIndex].Cells["controllernum"].Value.ToString();
            var categoryValues = new Dictionary<string, int>
    {
        { "تک دسته", 1 },
        { "دو دسته", 2 },
        { "سه دسته", 3 },
        { "چهار دسته", 4 }
    };
            return categoryValues[category];
        }

        // متد برای پخش صدا
        private void PlaySound()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("alert.wav");
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در پخش صدا: " + ex.Message);
            }
        }

        // متد برای متوقف کردن تایمر هنگام فشردن دکمه توقف
        private void StopTimer5(int rowIndex)
        {
            if (timers5.ContainsKey(rowIndex))
            {
                // متوقف کردن تایمر
                timers5[rowIndex].Change(Timeout.Infinite, Timeout.Infinite);
            }


            if (extraTimes5[rowIndex] == 0)
            {
            // زمان باقی‌مانده در جدول نمایش داده می‌شود، اما تایمر متوقف می‌شود
            dgv_ps5.Rows[rowIndex].Cells["remnant"].Value = TimeSpan.FromSeconds(remainingTimes5[rowIndex]).ToString(@"hh\:mm\:ss");
            }
            else
            {
               dgv_ps5.Rows[rowIndex].Cells["remnant"].Value = "+" + TimeSpan.FromSeconds(extraTimes5[rowIndex]).ToString(@"hh\:mm\:ss");
            }
            
        }











        //ps4

        private void CheckSelectedItem_ComboSysNum4()
        {
            if (sysnum_combo4.SelectedItem.ToString() != "انتخاب کنید")
            {
                controller_grp4.Enabled = true;
                sysnumcombovalue = sysnum_combo4.SelectedItem.ToString();

            }
            else
            {
                sysnumcombovalue = sysnum_combo4.SelectedItem.ToString();
                controller_grp4.Enabled = false;
                cost_grp4.Enabled = false;
            }

        }


        private void CheckSelected_RadController4()
        {
            foreach (Control control in controller_grp4.Controls)
            {
                if (control is RadioButton radioButton && radioButton.Checked)
                {
                    radcontrollervalue = radioButton.Text;
                    break; // Exit the loop after finding the checked radio button
                }
            }
        }

        private void CheckSelected_RadCost4()
        {
            foreach (Control control in cost_grp4.Controls)
            {
                if (control is RadioButton radioButton && radioButton.Checked)
                {
                    switch (radioButton.Name)
                    {
                        case "radtime_cost4":
                            if (txtbxtime_cost4.Text.Length > 0)
                            {
                                costtimevalue = Convert.ToInt32(txtbxtime_cost4.Text);
                                costadditionaltimevalue4 = (txtbxadditionaltime_cost4.Text.Length > 0 ? int.Parse(txtbxadditionaltime_cost4.Text) : 0);
                                return;
                            }
                            break;
                        case "radmoney_cost4":
                            if (txtbxmoney_cost4.Text.Length > 0)
                            {
                                // دریافت مبلغ ورودی
                                double moneyCost = Convert.ToDouble(txtbxmoney_cost4.Text);

                                // دریافت نرخ هر دقیقه برای تعداد جوی‌استیک‌های انتخاب‌شده
                                int radCategory = GetCategoryFromRadController4();
                                double ratePerMinute = dataDict4[radCategory];

                                // محاسبه زمان بر اساس مبلغ ورودی
                                costtimevalue = (int)((moneyCost / ratePerMinute)) / 60;

                                // ذخیره مبلغ ورودی
                                costmoneyvalue = (int)moneyCost;

                                // زمان اضافی (اگر وجود دارد)
                                costadditionaltimevalue4 = (txtbxadditionaltime_cost4.Text.Length > 0 ? int.Parse(txtbxadditionaltime_cost4.Text) : 0);
                                return;
                            }
                            break;
                        case "radfree_cost4":
                            costfreevalue = true;
                            costtimevalue = 0; // زمان آزاد
                            costadditionaltimevalue4 = (txtbxadditionaltime_cost4.Text.Length > 0 ? int.Parse(txtbxadditionaltime_cost4.Text) : 0);
                            return;
                    }
                }
            }
        }

        // Helper method to get the category (number of joysticks) from the selected radio button
        private int GetCategoryFromRadController4()
        {
            var categoryValues = new Dictionary<string, int>
    {
        { "تک دسته", 1 },
        { "دو دسته", 2 },
        { "سه دسته", 3 },
        { "چهار دسته", 4 }
    };

            return categoryValues[radcontrollervalue];
        }


        // دیکشنری تایمرها، زمان باقی‌مانده و زمان اولیه
        private Dictionary<int, System.Threading.Timer> timers4 = new Dictionary<int, System.Threading.Timer>();
        private Dictionary<int, int> remainingTimes4 = new Dictionary<int, int>();
        private Dictionary<int, int> extraTimes4 = new Dictionary<int, int>(); // زمان اضافه‌شده پس از رسیدن به صفر
        private Dictionary<int, int> initialTimes4 = new Dictionary<int, int>();
        private Dictionary<int, bool> isTimeExpired4 = new Dictionary<int, bool>(); // وضعیت اتمام زمان

        // متد برای افزودن ردیف جدید
        private void Add_datarow4()
        {
            var categoryValues = new Dictionary<string, int>
    {
        { "تک دسته", 1 },
        { "دو دسته", 2 },
        { "سه دسته", 3 },
        { "چهار دسته", 4 }
    };

            if (categoryValues.TryGetValue(radcontrollervalue, out int radcontrollervalueASnum))
            {
                // بررسی تکراری نبودن شماره سیستم
                foreach (DataGridViewRow row in dgv_ps4.Rows)
                {
                    if (row.Cells[0].Value.ToString() == sysnumcombovalue.ToString())
                    {
                        MessageBox.Show("این شماره سیستم قبلاً وارد شده است.");
                        return;
                    }
                }

                // محاسبه زمان کل بر اساس دقیقه و ثانیه
                int totalTimeInMinutes = costtimevalue;
                int totalTimeInSeconds = totalTimeInMinutes * 60;

                // اگر زمان آزاد است
                if (costfreevalue)
                {
                    totalTimeInMinutes = 0; // زمان آزاد
                    totalTimeInSeconds = 0;
                }

                // کم کردن زمان اضافه از زمان باقی‌مانده
                if (costadditionaltimevalue4 > 0)
                {
                    int extraTimeInSeconds = costadditionaltimevalue4 * 60;
                    totalTimeInSeconds -= extraTimeInSeconds;

                    // اطمینان از اینکه زمان باقی‌مانده منفی نشود
                    if (totalTimeInSeconds < 0)
                    {
                        totalTimeInSeconds = 0;
                    }
                }

                // محاسبه هزینه بر اساس نرخ هر ثانیه
                double ratePerSecond = dataDict4[radcontrollervalueASnum];
                double costPerSystem = (ratePerSecond * totalTimeInMinutes) * 60;
                costPerSystem = Math.Round(costPerSystem, 15);

                // افزودن ردیف به DataGridView
                int rowIndex;
                if (costadditionaltimevalue4 == 0)
                {
                    rowIndex = dgv_ps4.Rows.Add(
                        sysnumcombovalue,
                        radcontrollervalue,
                        costfreevalue ? "آزاد" : $"{totalTimeInMinutes} دقیقه", // زمان کل
                        DateTime.Now.ToString("H:m:s"), // زمان شروع
                        "0", // مبلغ اولیه
                        totalTimeInSeconds.ToString(),
                        "0" // زمان باقی‌مانده
                                                ,
                        "0"
                        ,
                        "0"
                    );
                }
                else
                {
                    TimeSpan time = TimeSpan.FromMinutes(costadditionaltimevalue4);
                    DateTime sttime = DateTime.Now - time;

                    rowIndex = dgv_ps4.Rows.Add(
                        sysnumcombovalue,
                        radcontrollervalue,
                        costfreevalue ? "آزاد" : $"{totalTimeInMinutes} دقیقه", // زمان کل
                        sttime.ToString("H:m:s"), // زمان شروع
                        "0", // مبلغ اولیه
                        totalTimeInSeconds.ToString(),
                        "0" // زمان باقی‌مانده
                                                ,
                        "0"
                        ,
                        "0"
                    );
                }

                // ذخیره اطلاعات در دیکشنری‌ها
                remainingTimes4[rowIndex] = totalTimeInSeconds;
                initialTimes4[rowIndex] = totalTimeInSeconds;
                extraTimes4[rowIndex] = 0; // مقدار اضافه‌شده از صفر شروع شود
                isTimeExpired4[rowIndex] = false;

                // ایجاد و شروع تایمر
                System.Threading.Timer timer = new System.Threading.Timer(UpdateTimeRemaining4, rowIndex, 1000, 1000);
                timers4[rowIndex] = timer;
            }
            else
            {
                MessageBox.Show("دسته انتخابی نامعتبر است.");
            }
        }

        // متد برای به‌روزرسانی زمان باقی‌مانده
        private void UpdateTimeRemaining4(object state)
        {
            int rowIndex = (int)state;

            // بررسی معتبر بودن ردیف
            if (rowIndex >= dgv_ps4.Rows.Count || dgv_ps4.Rows[rowIndex].Cells["remnant4"] == null)
            {
                Console.WriteLine($"Invalid rowIndex: {rowIndex}");
                return;
            }

            if (!remainingTimes4.ContainsKey(rowIndex)) return;

            if (remainingTimes4[rowIndex] > 0)
            {
                remainingTimes4[rowIndex]--;

                // محاسبه مبلغ بر اساس زمان باقی‌مانده
                double ratePerSecond = dataDict4[GetCategoryFromRowIndex4(rowIndex)];
                double costPerSystem = ratePerSecond * (initialTimes4[rowIndex] - remainingTimes4[rowIndex]);
                costPerSystem = Math.Round(costPerSystem, 15);



                // به‌روزرسانی مبلغ در DataGridView


                // تبدیل مقدار فعلی سلول به عدد
                double currentValue;
                if (double.TryParse(dgv_ps4.Rows[rowIndex].Cells["moneycost4"].Value?.ToString(), out currentValue))
                {
                    // انجام محاسبه
                    double newValue = currentValue + ratePerSecond;

                    // ارسال مقدار جدید به سلول با فرمت دو رقم اعشار
                    dgv_ps4.Rows[rowIndex].Cells["moneycost4"].Value = newValue.ToString("F2");
                }
                else
                {
                    // اگر مقدار فعلی سلول قابل تبدیل به عدد نباشد، می‌توانید یک مقدار پیش‌فرض قرار دهید یا خطا مدیریت کنید
                    dgv_ps4.Rows[rowIndex].Cells["moneycost4"].Value = costPerSystem.ToString("F2");
                }





                // به‌روزرسانی زمان باقی‌مانده
                dgv_ps4.Rows[rowIndex].Cells["remnant4"].Value = TimeSpan.FromSeconds(remainingTimes4[rowIndex]).ToString(@"hh\:mm\:ss");

                if (remainingTimes4[rowIndex] == 0)
                {
                    isTimeExpired4[rowIndex] = true;
                    PlaySound();





                    string pattern = @"\d+(\.\d+)?"; // الگو برای تشخیص اعداد (اعشاری یا صحیح)

                    Match match = Regex.Match(dgv_ps4.Rows[rowIndex].Cells["wholetime4"].Value.ToString(), pattern);

                    if (match.Success)
                    {
                        // اگر عدد پیدا شد، آن را به یک متغیر منتقل کنید
                        string numberString = match.Value;
                        int number = int.Parse(numberString); // تبدیل به عدد
                        number *= 60;
                        dgv_ps4.Rows[rowIndex].Cells["moneycost4"].Value = (ratePerSecond * number).ToString("F2");

                    }
                    else
                    {

                    }


                }
            }
            else
            {
                extraTimes4[rowIndex]++;

                // محاسبه مبلغ اضافی
                double extraMoney = CalculateExtraMoney4(rowIndex);

                // به‌روزرسانی مبلغ اضافی
                dgv_ps4.Rows[rowIndex].Cells["extramoney4"].Value = extraMoney.ToString("F2");

                // محاسبه مبلغ نهایی (moneycost + extramoney)
                double moneyCost = double.Parse(dgv_ps4.Rows[rowIndex].Cells["moneycost4"].Value.ToString());
                double finalMoney = moneyCost + extraMoney;

                // به‌روزرسانی مبلغ نهایی
                dgv_ps4.Rows[rowIndex].Cells["finalmoney4"].Value = finalMoney.ToString("F2");

                // به‌روزرسانی زمان اضافی
                dgv_ps4.Rows[rowIndex].Cells["remnant4"].Value = "+" + TimeSpan.FromSeconds(extraTimes4[rowIndex]).ToString(@"hh\:mm\:ss");
            }

            // ادامه تایمر
            timers4[rowIndex].Change(1000, 1000);
        }



        // متد برای محاسبه مبلغ اضافی
        private double CalculateExtraMoney4(int rowIndex)
        {
            int extraSeconds = extraTimes4[rowIndex];
            int radCategory = GetCategoryFromRowIndex4(rowIndex);
            double ratePerSecond = dataDict4[radCategory];

            double extraMoney = ratePerSecond * extraSeconds;
            return extraMoney;
        }

        // دریافت دسته از ردیف
        private int GetCategoryFromRowIndex4(int rowIndex)
        {
            string category = dgv_ps4.Rows[rowIndex].Cells["controllernum4"].Value.ToString();
            var categoryValues = new Dictionary<string, int>
    {
        { "تک دسته", 1 },
        { "دو دسته", 2 },
        { "سه دسته", 3 },
        { "چهار دسته", 4 }
    };
            return categoryValues[category];
        }

        // متد برای متوقف کردن تایمر هنگام فشردن دکمه توقف
        private void StopTimer4(int rowIndex)
        {
            if (timers4.ContainsKey(rowIndex))
            {
                // متوقف کردن تایمر
                timers4[rowIndex].Change(Timeout.Infinite, Timeout.Infinite);
            }


            if (extraTimes4[rowIndex] == 0)
            {
                // زمان باقی‌مانده در جدول نمایش داده می‌شود، اما تایمر متوقف می‌شود
                dgv_ps4.Rows[rowIndex].Cells["remnant4"].Value = TimeSpan.FromSeconds(remainingTimes4[rowIndex]).ToString(@"hh\:mm\:ss");
            }
            else
            {
                dgv_ps4.Rows[rowIndex].Cells["remnant4"].Value = "+" + TimeSpan.FromSeconds(extraTimes4[rowIndex]).ToString(@"hh\:mm\:ss");
            }

        }

























        private void sysnum_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckSelectedItem_ComboSysNum5();
        }





        private void Form1_Load(object sender, EventArgs e)
        {
            AddSyscomboItem();

            if (File.Exists(ps4) == false && File.Exists(ps5) == false)
            {
                MessageBox.Show("فایل یافت نشد!");
                this.Close();
            }
            else
            {
                // خواندن فایل و پردازش هر خط
                string[] lines4 = File.ReadAllLines(ps4);

                foreach (var line in lines4)
                {
                    // حذف فضاهای اضافی
                    string cleanedLine = line.Trim();

                    // تقسیم خط به دو بخش: کلید و مقدار
                    string[] parts = cleanedLine.Split('-');

                    if (parts.Length == 2)
                    {
                        // تبدیل کلید به عدد صحیح و مقدار به عدد اعشاری
                        if (int.TryParse(parts[0], out int key) && double.TryParse(parts[1], out double value))
                        {
                            // اضافه کردن به دیکشنری
                            dataDict4[key] = value;

                        }
                    }
                }


                string[] lines5 = File.ReadAllLines(ps5);
                foreach (var line in lines5)
                {
                    // حذف فضاهای اضافی
                    string cleanedLine = line.Trim();

                    // تقسیم خط به دو بخش: کلید و مقدار
                    string[] parts = cleanedLine.Split('-');

                    if (parts.Length == 2)
                    {
                        // تبدیل کلید به عدد صحیح و مقدار به عدد اعشاری
                        if (int.TryParse(parts[0], out int key) && double.TryParse(parts[1], out double value))
                        {
                            // اضافه کردن به دیکشنری
                            dataDict5[key] = value;

                        }
                    }
                }
            }
            addcostviwe();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            cost_grp5.Enabled = true;
        }

        private void rad_crt2_5_CheckedChanged(object sender, EventArgs e)
        {
            cost_grp5.Enabled = true;
        }

        private void rad_crt3_5_CheckedChanged(object sender, EventArgs e)
        {
            cost_grp5.Enabled = true;
        }

        private void rad_crt4_5_CheckedChanged(object sender, EventArgs e)
        {
            cost_grp5.Enabled = true;
        }

        private void submit_btn_Click(object sender, EventArgs e)
        {
            // بررسی خالی نبودن فیلدهای ورودی
            if (string.IsNullOrEmpty(txtbxtime_cost5.Text) && string.IsNullOrEmpty(txtbxmoney_cost5.Text))
            {
                MessageBox.Show("لطفاً زمان یا مبلغ را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // جلوگیری از ادامه عملیات
            }

            // اگر یکی از فیلدها پر باشد، ادامه عملیات
            CheckSelected_RadController5();
            CheckSelected_RadCost5();
            Add_datarow5();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtbxtime_cost_KeyPress(object sender, KeyPressEventArgs e)
        {
        
                // بررسی اینکه آیا کلید فشرده شده یک عدد است
                if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true; // جلوگیری از وارد کردن کاراکتر غیر عددی

                    // نمایش پیام هشدار با استفاده از ToolTip
                    tp_time_warrning.Show("لطفاً فقط عدد وارد کنید.", txtbxtime_cost5, 0, -20, 2000); // پیام برای 2 ثانیه نمایش داده می‌شود
                }

        }



        private void button2_Click(object sender, EventArgs e)
        {
            // انتخاب ردیف از DataGridView
            int rowIndex = dgv_ps5.CurrentRow.Index;

            // متوقف کردن تایمر مربوط به ردیف انتخابی
            StopTimer5(rowIndex);
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            // انتخاب ردیف از DataGridView
            int rowIndex = dgv_ps5.CurrentRow.Index;

            // حذف ردیف از DataGridView
            dgv_ps5.Rows.RemoveAt(rowIndex);

            // حذف تایمر مربوط به ردیف انتخابی
            if (timers5.ContainsKey(rowIndex))
            {
                timers5[rowIndex].Dispose();  // از بین بردن تایمر
                timers5.Remove(rowIndex); // حذف تایمر از دیکشنری
            }

            // حذف زمان باقی‌مانده و زمان اولیه
            if (remainingTimes5.ContainsKey(rowIndex))
            {
                remainingTimes5.Remove(rowIndex);
            }

            if (initialTimes5.ContainsKey(rowIndex))
            {
                initialTimes5.Remove(rowIndex);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void tp_time_warrning_Popup(object sender, PopupEventArgs e)
        {

        }

        private void submit_btn4_Click(object sender, EventArgs e)
        {
            // بررسی خالی نبودن فیلدهای ورودی
            if (string.IsNullOrEmpty(txtbxtime_cost4.Text) && string.IsNullOrEmpty(txtbxmoney_cost4.Text))
            {
                MessageBox.Show("لطفاً زمان یا مبلغ را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // جلوگیری از ادامه عملیات
            }

            // اگر یکی از فیلدها پر باشد، ادامه عملیات
            CheckSelected_RadController4();
            CheckSelected_RadCost4();
            Add_datarow4();
        }

        private void sysnum_combo4_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckSelectedItem_ComboSysNum4();
        }

        private void btn_stop4_Click(object sender, EventArgs e)
        {
            // انتخاب ردیف از DataGridView
            int rowIndex = dgv_ps4.CurrentRow.Index;

            // متوقف کردن تایمر مربوط به ردیف انتخابی
            StopTimer4(rowIndex);
        }

        private void btn_delete4_Click(object sender, EventArgs e)
        {
            // انتخاب ردیف از DataGridView
            int rowIndex = dgv_ps4.CurrentRow.Index;

            // حذف ردیف از DataGridView
            dgv_ps4.Rows.RemoveAt(rowIndex);

            // حذف تایمر مربوط به ردیف انتخابی
            if (timers4.ContainsKey(rowIndex))
            {
                timers4[rowIndex].Dispose();  // از بین بردن تایمر
                timers4.Remove(rowIndex); // حذف تایمر از دیکشنری
            }

            // حذف زمان باقی‌مانده و زمان اولیه
            if (remainingTimes4.ContainsKey(rowIndex))
            {
                remainingTimes4.Remove(rowIndex);
            }

            if (initialTimes4.ContainsKey(rowIndex))
            {
                initialTimes4.Remove(rowIndex);
            }
        }

        private void rad_crt1_4_CheckedChanged(object sender, EventArgs e)
        {
            cost_grp4.Enabled = true;

        }

        private void rad_crt2_4_CheckedChanged(object sender, EventArgs e)
        {
            cost_grp4.Enabled = true;

        }

        private void rad_crt3_4_CheckedChanged(object sender, EventArgs e)
        {
            cost_grp4.Enabled = true;

        }

        private void rad_crt4_4_CheckedChanged(object sender, EventArgs e)
        {
            cost_grp4.Enabled = true;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "MP3 Files|*.mp3";
            openFileDialog1.Title = "Select an MP3 File";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string mp3FilePath = openFileDialog1.FileName;

                // Always save as "alert.wav" in the application's directory
                string appDirectory = Application.StartupPath; // Get the exe's folder
                string wavFilePath = Path.Combine(appDirectory, "alert.wav");

                ConvertMp3ToWav(mp3FilePath, wavFilePath);

                MessageBox.Show($"Conversion successful!\nSaved as: {wavFilePath}",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ConvertMp3ToWav(string mp3Path, string wavFilePath)
        {
            // Check if "alert.wav" already exists and delete it to replace
            if (File.Exists(wavFilePath))
            {
                File.Delete(wavFilePath);
            }

            // Convert MP3 to WAV
            using (var mp3Reader = new NAudio.Wave.Mp3FileReader(mp3Path))
            using (var pcmStream = NAudio.Wave.WaveFormatConversionStream.CreatePcmStream(mp3Reader))
            using (var wavWriter = new NAudio.Wave.WaveFileWriter(wavFilePath, pcmStream.WaveFormat))
            {
                pcmStream.CopyTo(wavWriter);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // جلوگیری از وارد کردن کاراکتر غیر عددی

                // نمایش پیام هشدار با استفاده از ToolTip
                tp_time_warrning.Show("لطفاً فقط عدد وارد کنید.", txtbxtime_cost5, 0, -20, 2000); // پیام برای 2 ثانیه نمایش داده می‌شود
            }
        }
        string combo_setcost_type_val;
        int combo_setcost_sysnum_val;
        string moneycosttxt_val;
        private void button1_Click(object sender, EventArgs e)
        {
            if (combo_setcost_sysnum.SelectedItem.ToString() != "انتخاب کنید" && combo_setcost_sysnum.SelectedItem.ToString() != "")
            {
                combo_setcost_sysnum_val = int.Parse(combo_setcost_sysnum.SelectedItem.ToString());
                if (combo_setcost_type.SelectedItem.ToString() != "انتخاب کنید" && combo_setcost_sysnum.SelectedItem.ToString() != "")
                {
                    combo_setcost_type_val = combo_setcost_type.SelectedItem.ToString() + ".txt";
                    if (moneycosttxt.Text.Length>0)
                    {
                        moneycosttxt_val = moneycosttxt.Text;
                        moneycosttxt_val = (Math.Round((decimal.Parse(moneycosttxt_val) / 60) / 60, 15)).ToString();


                        UpdateFile(combo_setcost_type_val,combo_setcost_sysnum_val,moneycosttxt_val);
                    }
                }
            }
        }
        private void UpdateFile(string fileName, int number, string newValue)
        {
            try
            {
                // Read all lines from the file
                string[] lines = File.ReadAllLines(fileName);

                // Loop through each line and find the one that starts with the number (1-4)
                for (int i = 0; i < lines.Length; i++)
                {
                    // Check if the line starts with the number followed by '-'
                    if (lines[i].StartsWith(number.ToString() + "-"))
                    {
                        // Replace the current value after the dash with the new value
                        lines[i] = number + "-" + newValue;
                        break; // Exit the loop once the line is updated
                    }
                }

                // Write the modified lines back to the file
                File.WriteAllLines(fileName, lines);
                MessageBox.Show("File updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Restart();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Process.Start("calc.exe");
        }

        private void combo_setcost_sysnum_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }
    }
}
