{HTTP请求回调}
function onhttprequest(){
	var(url, '', method, '', servicename, '');
	http.getrequesturl(url);
	http.getrequestmethod(method);
	http.getservicename(servicename);
	str.tolower(servicename, servicename);
	http.setstatuscode(200);
	
	out(url);
	var(ip, '');
	http.getremoteip(ip);
	out(ip);
	out(http.getremoteport());
	if(str.equals(servicename, 'login')){
	    onloginrequest();
	}
	else if(str.equals(servicename, 'wechat')) {
	    onwechatrequest();
	}
	else if(str.equals(servicename, 'hard')) {
	    http.hardrequest();
	}
	else if(str.equals(servicename, 'easy')) {
	    http.easyrequest('easy');
	}
}

{登录请求}
function onloginrequest(){
	var(name, '', pwd, '');
	http.querystring(name, 'name');
	http.querystring(pwd, 'pwd');
	http.write('name:', name, '<br/>');
	http.write('pwd:', pwd, '<br/>');
	if(str.equals(name, 'admin')
	 && str.equals(pwd, '111111')){
		http.write('Login Success!'); 
	}
	else{
		http.write('Login Fail!');
	}
}

{检查脚本状态}
function oncheckscript(){
	while(isappalive()){
		sleep(10000);
		http.checkscript();
	}
}

{微信请求}
function onwechatrequest(){
	http.write('Welcome to WeChat!');
}

{HTTP服务准备启动事件}
function onhttpserverstarting(filename){
	out('执行脚本文件:', filename);	
	http.addport(8086);
	createthread('oncheckscript();');
}

{HTTP服务启动成功事件}
function onhttpserverstart(maxThreadNum, minThreadNum){
	out('服务启动成功...');
	out('最大线程数:', maxThreadNum);
	out('最小空闲线程数:', minThreadNum);
}

{HTTP服务启动失败事件}
function onhttpserverstartfail(error){
	out('服务启动失败...');
	out(error);
}