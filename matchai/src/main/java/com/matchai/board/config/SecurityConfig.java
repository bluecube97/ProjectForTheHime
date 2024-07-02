package com.matchai.board.config;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.security.web.SecurityFilterChain;
import org.springframework.web.servlet.config.annotation.CorsRegistry;
import org.springframework.web.servlet.config.annotation.ResourceHandlerRegistry;
import org.springframework.web.servlet.config.annotation.WebMvcConfigurer;

import com.matchai.board.service.CustomOAuth2UserService;

import javax.servlet.http.HttpServletResponse;

@Configuration
@EnableWebSecurity
public class SecurityConfig implements WebMvcConfigurer {

    @Autowired
    private CustomOAuth2UserService customOAuth2UserService;

    // SecurityFilterChain 빈 생성
    @Bean
    public SecurityFilterChain securityFilterChain(HttpSecurity http) throws Exception {
        http.authorizeRequests()
                // 인증 없이 접근 허용되는 경로 설정
                .antMatchers("/", "/user/**", "/board/**", "/resource/**").permitAll()
                // 그 외의 모든 요청은 인증 필요
                .anyRequest().authenticated().and().oauth2Login()
                // 사용자 정의 로그인 페이지
                .loginPage("/user/login")
                // 로그인 성공 시 이동할 페이지
                .defaultSuccessUrl("/board/main", true)
                // 로그인 실패 시 이동할 페이지
                .failureUrl("/user/loginFailure").userInfoEndpoint()
                // 사용자 정보 서비스 설정
                .userService(customOAuth2UserService).and().and().logout()
                // 로그아웃 URL 설정
                .logoutUrl("/user/logout")
                // 로그아웃 성공 시 리다이렉트할 URL 설정
                .logoutSuccessUrl("/board/main")
                // 세션 무효화
                .invalidateHttpSession(true)
                // 쿠키 삭제
                .deleteCookies("JSESSIONID").and()
                // CSRF 보호 비활성화 (개발 중일 때만)
                .csrf().disable()
                // CORS 설정 추가
                .cors();

        return http.build();
    }

    // 비밀번호 암호화를 위한 BCryptPasswordEncoder 빈 생성
    @Bean
    public PasswordEncoder passwordEncoder() {
        return new BCryptPasswordEncoder();
    }

    // 정적 리소스 핸들러 설정
    @Override
    public void addResourceHandlers(ResourceHandlerRegistry registry) {
        registry.addResourceHandler("/resource/**").addResourceLocations("/resource/");
    }

    // CORS 설정 추가
    @Override
    public void addCorsMappings(CorsRegistry registry) {
        registry.addMapping("/**").allowedOriginPatterns("*") // allowedOrigins 대신 allowedOriginPatterns 사용
                .allowedMethods("GET", "POST", "PUT", "DELETE", "OPTIONS").allowedHeaders("*").allowCredentials(true);
    }
}
