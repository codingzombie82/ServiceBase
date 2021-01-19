Blazor, MVC, Web Api를 사용한 프로젝트


작업내용
[1] dotnet core 3.1 프로젝트 생성 (blazor server)

[2] database 프로젝트 생성

[3] 닷넷  클래스 라이브러리 프로젝트 생성

[4] jwt 인증 authentication 적용

패키지 설치

Microsoft.AspNetCore.Authentication.JwtBearer

Microsoft.EntityFrameworkCore

Microsoft.EntityFrameworkCore.SqlServer

Microsoft.EntityFrameworkCore.Tools

[4] 로컬에 빈 데이터 베이스 게시

[5] appsettings.json 에 ConnectionString 적용

[6] servicebase.models에 User 클래스 생성

[7] servicebase 종속성에 servicebase.models 프로젝트 추가

[8] data 폴더에 ApplicationDbContext.cs 생성

[9] startup.cs 에 DBContext 주입

[10] blazor로 프로젝트를 시작했기때문에 services.AddControllers(); 추가

[11] 마이그래이션 하기
- 보기 -> 다른창 -> 패키지 관리자 콘솔을 열어 Add-Migration Users
-> 마이그래이션 폴더 생성되고 Migration을 상속 받은 Users 생성됨
-> Users 테이블을 생성하기 위한 Update-Database 적용 -> 데이터 베이스에 Users 테이블 확인

[12] Services 폴더 생성
TokenBuilder.cs
ITokenBuilder.cs 파일 추가

[12] ConfigureServices에 TokenBuilder, ITokenBuilder 주입 및 인증을 구성

[13] endpoints 생성
- apis 폴더 생성 
- AuthenticationController.cs 파일 추가 Api 용으로 만듬
- 생성자에  ApplicationDbContext, ITokenBuilder 읽기 전용으로 추가
- login api 적용 ( 테스트용으로 DB에 있는 패스워드 정보는 고정으로 하나 차후 암호화해서 적용)
- VerifyToken api 적용 : 토큰 유효성 확인하는 함수


[14] 최초 Blazor 프로젝트로 생성을 했기 때문에 MVC를 통한 api가 정상적으로 호출되는 지 확인 (Blazor + MVC 혼용사용)
- Controllers 폴더 생성
- HomeController 생성
- Views 폴더 생성
- Views 폴더 아래 -> Home 폴더 생성 -> HomeController에 index.cshtml 보기 파일 생성
- MVC와 Blazor를 동시 사용하기 위한 endpoint의 라우터 설정 (startup.cs)
- app.UseAuthorization(); 인증처리를 위한 함수 추가 (startup.cs)

[15] 테스트 확인
API 확인
ex)Post https://localhost:44344/api/authentication/login (토큰 정상적으로 가져오는 지 확인 성공하면 200 OK, 실패하면 400 Bad Request)
성공한 키를 가지고 Verify 체크
ex)Get https://localhost:44344/api/authentication/verify (헤더에 Authorization / Bearer #위에서 얻어온 토큰 추가#)
: 성공시 204 No Content 실패시 401Unauthorized

[16] 토큰 인증된 사용자만 API 사용처리
- app.UseAuthentication(); 토큰 인증처리 체계 등록을 위한 함수 추가 (startup.cs)
- 테스트를 위해 기본 API 추가  ValuesController (Api)
- Api GET [Authorize] 어노테이션 추가
- 테스트 Headers에 key(Authorization) Value(인증된 토큰 추가) Bearer #토큰#
- Api 호출 테스트 정상적인 방식이면 200 ok , 실패시 401 Unauthorized 호출됨
- 아직 토큰에 대한 Expire 적용 전 단계
- Role 에 따른 함수 호출 차이 적용 전 단계










































