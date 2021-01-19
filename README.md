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
- 보기 -> 다른창 -> 패키지 관리자 콘솔을 열어 
Add-Migration Users
: 마이그래이션 폴더 생성되고 Migration을 상속 받은 Users 생성됨
Users 테이블을 생성하기 위한 
Update-Database 적용 -> 데이터 베이스에 Users 테이블 확인

















