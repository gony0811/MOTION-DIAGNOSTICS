using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ACS.SPiiPlusNET;
using Serilog;

namespace Device.Driver
{ 

    public class ACSDriver
    {
        public bool IsSimulationMode { get; set; } = false; 

        public readonly Api Api;


        public bool IsConnected => Api.IsConnected;
        public string ErrorMessage { get; set; }



        public ACSDriver(ILogger logger, string connectionString)
        {
            Api = new Api();
            this.connectionString = connectionString; 
            this.logger = logger;

            Connection();
        }

        private void Connection()
        {
            string[] resultTokens = this.connectionString.Split(',');

            this.logger.Information($"connection() : connectionString={this.connectionString}");

            try
            {
                switch (resultTokens[0])
                {
                    case "Serial":
                        int channel = Convert.ToInt32(resultTokens[1]);
                        int rate = Convert.ToInt32(resultTokens[2]);
                        Api.OpenCommSerial(channel: channel, rate: rate);
                        this.logger.Information($"communication=serial,port={channel},baudrate={rate}");
                        break;

                    case "Ethernet":
                        string address = resultTokens[1];
                        int port = Convert.ToInt32(resultTokens[2]);
                        Api.OpenCommEthernet(address: address, port: port);
                        this.logger.Information($"communication=ethernet,address={address},port={port}");
                        break;

                    case "Pci":
                        int slotNumber = Convert.ToInt32(resultTokens[1]);

                        Api.OpenCommPCI(slotNumber: slotNumber);
                        this.logger.Information($"communication=pci,slotNumber={slotNumber}");
                        break;

                    case "Simulator":
                        Api.OpenCommSimulator();
                        this.logger.Information($"communication=simulator");
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "ACS connection fail.");
            }


        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    // Dispose managed resources.                    
                }


                // Dispose unmanaged resources.
                if (Api != null)
                {
                    Api.CloseComm();
                }

                isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #region Private Methods        
        /// <summary>
        /// 에러 메시지 확인
        /// </summary>
        /// <param name="ex"></param>
        private void _ErrorComMsg(COMException ex)
        {
            string Str = "Error from " + ex.Source + "\n\r";
            Str = Str + ex.Message + "\n\r";
            Str = Str + "HRESULT:" + String.Format("0x{0:X}", (ex.ErrorCode));
            this.logger.Error(Str);
        }

        /// <summary>
        /// ACS 에러 메시지 확인
        /// </summary>
        /// <param name="ex"></param>
        private void _ErrorAcsMsg(ACSException ex)
        {
            string Str = "Error from " + ex.Source + "\n\r";
            Str = Str + ex.Message + "\n\r";
            Str = Str + "HRESULT:" + String.Format("0x{0:X}", (ex.ErrorCode));
            this.logger.Error(Str);
        }
        #endregion

        #region Service Communication Methods
        /// <summary>
        /// SPiiPlus C Library 버전 문자열 정보 획득: e.g. ) 6.67.0.0
        /// </summary>
        /// <returns></returns>
        public string GetLibraryVersion()
        {
            return "";
            //return Inno6NumericManager.GetVersionString(Ch.GetLibraryVersion());
        }

        /// <summary>
        /// 닷넷 라이브러리 버전 정보를 획득: e.g. ) xx.xx.xx.xx
        /// </summary>
        /// <returns></returns>
        public string GetNetLibraryVersion()
        {
            return "";
            //return Inno6NumericManager.GetVersionString(Ch.GetNETLibraryVersion());
        }

        /// <summary>
        /// 기본 타임아웃 값을 확인
        /// </summary>
        /// <returns></returns>
        public int GetDefaultTimeout()
        {
            return Api.GetDefaultTimeout();
        }

        /// <summary>
        /// 에러코드에 해당하는 내용을 문자열로 확인
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public string GetErrorStringFromErrorCode(int errorCode)
        {
            return Api.GetErrorString(errorCode);
        }
        #endregion

        #region ACSPL+ Program Management Methods
        /// <summary>
        /// 버퍼 추가 
        /// </summary>
        /// <param name="programBuffer"></param>
        /// <param name="programLines"></param>
        public void AppendBuffer(int programBufferIndex, string programLines)
        {
            Api.AppendBuffer((ProgramBuffer)programBufferIndex, programLines);
        }
        /// <summary>
        /// 버퍼를 라인 시작 부투 끝까지 범위를 지정해서 지운다.
        /// </summary>
        /// <param name="programBuffer"></param>
        /// <param name="fromLine"></param>
        /// <param name="toLine"></param>
        public void ClearBuffer(int programBufferIndex, int fromLine, int toLine)
        {
            Api.ClearBufferAsync((ProgramBuffer)programBufferIndex, fromLine, toLine);
        }

        /// <summary>
        /// 버퍼 컴파일
        /// </summary>
        /// <param name="programBuffer"></param>
        public void CompileBuffer(int programBufferIndex)
        {
            Api.CompileBufferAsync((ProgramBuffer)programBufferIndex);
        }

        /// <summary>
        /// 버퍼 로드 
        /// </summary>
        /// <param name="programBuffer"></param>
        /// <param name="programLines"></param>
        public void LoadBuffer(int programBufferIndex, string programLines)
        {
            Api.LoadBufferAsync((ProgramBuffer)programBufferIndex, programLines);
        }
        /// <summary>
        /// 버퍼 프로그램을 파일에서 불러온다.
        /// </summary>
        /// <param name="fileName"></param>
        public void LoadBufferFromFile(string fileName)
        {
            Api.LoadBuffersFromFileAsync(fileName);
        }


        /// <summary>
        /// 버퍼 실행
        /// </summary>
        /// <param name="programBuffer"></param>
        /// <param name="label"></param>
        public void RunBuffer(int programBufferIndex, string label)
        {
            Api.RunBufferAsync((ProgramBuffer)programBufferIndex, null/*label*/);
        }

        /// <summary>
        /// 프로그램 버퍼 멈춤
        /// </summary>
        /// <param name="programBufferIndex"></param>
        public void StopBuffer(int programBufferIndex)
        {
            Api.StopBufferAsync((ProgramBuffer)programBufferIndex);
        }

        /// <summary>
        /// 버퍼 일시 정지
        /// </summary>
        /// <param name="programBufferIndex"></param>
        public void SuspendBuffer(int programBufferIndex)
        {
            Api.SuspendBufferAsync((ProgramBuffer)programBufferIndex);
        }

        /// <summary>
        /// 버퍼 업로드
        /// </summary>
        /// <param name="programBufferIndex"></param>
        public void UploadBuffer(int programBufferIndex)
        {
            Api.UploadBufferAsync((ProgramBuffer)programBufferIndex);
        }

        /// <summary>
        /// 버퍼 브레이크 포인트 지정
        /// </summary>
        /// <param name="programBufferIndex"></param>
        /// <param name="line"></param>
        public void SetBreakPoint(int programBufferIndex, int line)
        {
            Api.SetBreakpointAsync((ProgramBuffer)programBufferIndex, line);
        }

        /// <summary>
        /// 버퍼 브레이크 포인트 리스트 정보 획득
        /// </summary>
        /// <param name="programBufferIndex"></param>
        /// <returns></returns>
        public int[] GetBreakPointsList(int programBufferIndex)
        {
            return Api.GetBreakpointsList((ProgramBuffer)programBufferIndex);
        }

        /// <summary>
        /// 버퍼 브레이크 포인트 리스트 삭제
        /// </summary>
        /// <param name="programBufferIndex"></param>
        /// <param name="line"></param>
        public void ClearBreakPoints(int programBufferIndex, int line)
        {
            Api.ClearBreakpointsAsync((ProgramBuffer)programBufferIndex, line);
        }

        /// <summary>
        /// 프로그램 상태 확인
        /// </summary>
        /// <param name="programBufferIndex"></param>
        public void GetProgramState(int programBufferIndex)
        {
            ProgramStates states = Api.GetProgramState((ProgramBuffer)programBufferIndex);
        }

        #endregion

        #region ACS Variables

        /// <summary>
		/// 변수 읽기
		/// </summary>
		/// <param name="variableName"></param>
		/// <param name="programBufferIndex"></param>
		/// <param name="from1"></param>
		/// <param name="to1"></param>
		/// <param name="from2"></param>
		/// <param name="to2"></param>
		/// <returns></returns>
		public object ReadVariable(string variableName, int programBufferIndex = (int)ProgramBuffer.ACSC_NONE, int rowFrom = -1, int rowTo = -1, int columnFrom = -1, int columnTo = -1)
        {
            return Api.ReadVariable(variableName, (ProgramBuffer)programBufferIndex, rowFrom, rowTo, columnFrom, columnTo);
        }

        /// <summary>
        /// 변수 쓰기
        /// </summary>
        /// <param name="value"></param>
        /// <param name="variable"></param>
        /// <param name="programBufferIndex"></param>
        /// <param name="from1"></param>
        /// <param name="to1"></param>
        /// <param name="from2"></param>
        /// <param name="to2"></param>
        public void WriteVariable(string variable, object value, int programBufferIndex = (int)ProgramBuffer.ACSC_NONE, int rowFrom = -1, int rowTo = -1, int columnFrom = -1, int columnTo = -1)
        {
            Api.WriteVariableAsync(value, variable, (ProgramBuffer)programBufferIndex, rowFrom, rowTo, columnFrom, columnTo);
        }
        #endregion

        #region Communication
        /// <summary>
        /// CaptureComm
        /// </summary>
        public void CaptureComm()
        {
            Api.CaptureComm();
        }

        /// <summary>
        /// ReleaseComm
        /// </summary>
        public void ReleaseComm()
        {
            Api.ReleaseComm();
        }
        #endregion


        private string connectionString;
        private ILogger logger;
        private bool isDisposed = false;
        private const int kErrorStartNumber = 100;
        private const int kTimeoutFromSeconds = 60;

        private bool mDisposed;
        private Stopwatch Stopwatch = new Stopwatch();

        class Serial
        {
            public int Channel { get; set; }
            public int Rate { get; set; }
        }

        class Ethernet
        {
            public string Ip { get; set; }
            public int Port { get; set; }
        }

        class Pci
        {
            public int SlotNumber { get; set; }
        }
    }
}
