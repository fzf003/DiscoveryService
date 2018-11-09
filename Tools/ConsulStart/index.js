const { spawn } = require('child_process');

const bat = spawn('cmd.exe', ['/c', 'run.bat'], { shell: true, windowsHide: true });

///子进程数据输出
bat.stdout.on('data', (data) => {
  console.log(data.toString());
});

///错误日志信息数据输出
bat.stderr.on('data', (data) => {
  console.log(data.toString());
});

bat.on('exit', (code) => {
  console.log(`子进程退出码：${code}`);
});
