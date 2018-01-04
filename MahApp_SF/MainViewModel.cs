namespace MahApp_SF
{
    using System;
    using System.ComponentModel;
    using SoftFluent.Windows;

    class MainViewModel: AutoObject,IDataErrorInfo
    {
        [Category("Cannot goto Validation")]
        [DisplayName("TE")]
        [PropertyGridOptions(SortOrder = 1, EditorDataTemplateResourceKey = "TEEditor")]
        public float TE
        {
            get { return GetProperty<float>(); }
            set { SetProperty(value); }
        }

        #region fields
        const float gamma_H = 42.58f;
        const float MIN_DURATION = 0.001f;

        public int NumErrors { get; set; }

        [Browsable(false)]
        public string SequenceName { get; set; }

        [Browsable(false)]
        public float SF
        {
            get { return GetProperty<float>(); }
            set { SetProperty(value); }
        }

        [Browsable(false)]
        public float O1
        {
            get { return GetProperty<float>(); }
            set { SetProperty(value); }
        }

        
        [Browsable(false)]
        public float SP
        {
            get { return GetProperty<float>(); }
            set { SetProperty(value); }
        }

        [Browsable(false)]
        public int Mx { get; set; }

        [Browsable(false)]
        [PropertyGridOptions(SortOrder = 17, EditorDataTemplateResourceKey = "IntEditor")]
        public int My { get; set; }

        [Browsable(false)]
        public float FOVx { get; set; }

        [Browsable(false)]
        public float FOVy { get; set; }

        [Browsable(false)]
        public int Mz { get; set; }

        [Browsable(false)]
        public float SliceThickness { get; set; }

        [Browsable(false)]
        public float SliceGap { get; set; }



        [Browsable(false)]
        public float SW { get; set; }



        [Browsable(false)]
        public float D2 { get; set; }

        [Browsable(false)]
        public float D1 { get; set; }

        [Browsable(false)]
        public float D9 { get; set; }

        [Browsable(false)]
        public float D10 { get; set; }

        [Browsable(false)]
        public float EchoPosRatio { get; set; }

        [Browsable(false)]
        public float[] PCL2 { get; set; }

        [Browsable(false)]
        public float[] PCL1 { get; set; }

        [Browsable(false)]
        public float[] PCL0 { get; set; }

        [Browsable(false)]
        public float GxMax { get; set; }

        [Browsable(false)]
        public float GyMax { get; set; }

        [Browsable(false)]
        public float GzMax { get; set; }

        [Browsable(false)]
        public float OffsetRead { set; get; }

        [Browsable(false)]
        public float OffsetSlice { set; get; }
        #endregion

        public MainViewModel()
        {
            SequenceName = "SE";
            SF = 23;  // MHz
            O1 = 306000; // Hz
            SP = 3200;   // us

            TE = 2;    // ms D2
          
            SW = 20; // kHz
           
            Mx = 256;
            My = 192;

          

            float[] lst0 = new float[2];
            lst0[0] = 0; lst0[1] = 180;
            PCL0 = lst0;

            float[] lst1 = new float[2];
            lst1[1] = 0; lst1[1] = 0;
            PCL1 = lst1;

            float[] lst2 = new float[2];
            lst2[0] = 90; lst2[1] = 270;
            PCL2 = lst2;

            FOVx = 80;
            FOVy = 80;
            Mz = 1;
            SliceGap = 1;
            SliceThickness = 5;

            D2 = 3;

            D1 = 0.1f;
            D9 = 0.1f;
            D10 = 5.0f;
            EchoPosRatio = 0.5f;

            GxMax = 2.85f;
            GyMax = 2.80f;
            GzMax = 3.60f;

            OffsetRead = 0;
            OffsetSlice = 0;
        }

        #region Validation
        [Browsable(false)]
        public string Error { get { return string.Empty; } }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "TE")
                {
                    //D4 
                    if (TE / 2.0 - SP / 1000.0 - D9 * 4.0 - D2 - D1 - 0.005 < MIN_DURATION)
                    {
                        return "TE is too short (D4 < " + MIN_DURATION + "ms) ";
                    }
                    //D5
                    double ACQ = Mx / SW;
                    float DT2 = 0.3f;
                    if (TE / 2.0 - SP / 2000.0 - D1 - D9 * 2.0 - DT2 - ACQ * EchoPosRatio < MIN_DURATION)
                    {
                        return "TE is too short (D5 < " + MIN_DURATION + "ms) ";
                    }

                    // MinTE
                    double te1 = 2.0 * (0.002 + SP / 1000.0 + D1 + 4.0 * D9 + D2 + 0.005);
                    double te2 = 2.0 * (0.002 + SP / 2000.0 + D1 + 2.0 * D9 + DT2 + 0.01 * ACQ * EchoPosRatio);
                    double se_min_te = te1 > te2 ? te1 : te2;
                    if (TE < se_min_te)
                    {
                        return "TE is too short (TE should >= " + se_min_te + "ms) ";
                    }
                }

                return null;
            }
        }
        #endregion
    }
}
