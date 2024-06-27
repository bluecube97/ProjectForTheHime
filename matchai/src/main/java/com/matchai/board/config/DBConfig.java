package com.matchai.board.config;

import javax.sql.DataSource;

import org.apache.ibatis.session.SqlSessionFactory;
import org.mybatis.spring.SqlSessionFactoryBean;
import org.mybatis.spring.SqlSessionTemplate;
import org.mybatis.spring.annotation.MapperScan;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.context.ApplicationContext;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.PropertySource;

import com.zaxxer.hikari.HikariConfig;
import com.zaxxer.hikari.HikariDataSource;

@Configuration // 이 클래스가 Spring의 설정 클래스임을 나타냄(pom.xml 대신?)
@PropertySource("classpath:/application.properties") // application.properties 파일에서 설정을 읽어옴
@MapperScan(basePackages = "com.matchai.board.mapper") // MyBatis 매퍼 인터페이스를 스캔할 패키지 지정
// @MapperScan({"com.matchai.board.mapper", "com.game.baseball.api.mapper", "com.unity.game.mapper"}) // MyBatis 매퍼 인터페이스를 스캔할 패키지 지정
public class DBConfig {

	@Autowired // Spring이 자동으로 ApplicationContext를 주입, bean의 id갑 가져와서 쓸 수 있다..?
	private ApplicationContext context;

	@Bean // 이 메서드가 반환하는 객체를 Spring 컨텍스트에 빈으로 등록
	@ConfigurationProperties(prefix = "spring.datasource.hikari") // application.properties 파일의 HikariCP 설정을 사용
	public HikariConfig hikariConfig() {
		return new HikariConfig(); // HikariCP 설정을 담고 있는 HikariConfig 객체 생성
	}

	@Bean // DataSource 빈을 Spring 컨텍스트에 등록
	public DataSource dataSource() {
		return new HikariDataSource(hikariConfig()); // HikariCP 설정을 적용한 DataSource 생성
	}

	@Bean // SqlSessionFactory 빈을 Spring 컨텍스트에 등록
	public SqlSessionFactory session() throws Exception {
		SqlSessionFactoryBean sqlSessionFactoryBean = new SqlSessionFactoryBean();
		sqlSessionFactoryBean.setDataSource(dataSource()); // DataSource 설정
		sqlSessionFactoryBean.setMapperLocations(context.getResources("classpath:/mappers/*.xml")); // 매퍼 파일 위치 설정
		return sqlSessionFactoryBean.getObject(); // SqlSessionFactory 객체 반환
	}

	@Bean // SqlSessionTemplate 빈을 Spring 컨텍스트에 등록
	public SqlSessionTemplate sqlSession() throws Exception {
		return new SqlSessionTemplate(session()); // SqlSessionFactory 설정
	}

}
