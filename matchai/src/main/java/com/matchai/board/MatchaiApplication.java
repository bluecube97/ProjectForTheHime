package com.matchai.board;

import org.mybatis.spring.annotation.MapperScan;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.builder.SpringApplicationBuilder;
import org.springframework.boot.web.servlet.support.SpringBootServletInitializer;
import org.springframework.context.annotation.ComponentScan;

@SpringBootApplication
@MapperScan({"com.matchai.board.mapper", "com.unity.game.mapper"})
@ComponentScan({"com.matchai.board", "com.unity.game"})
public class MatchaiApplication extends SpringBootServletInitializer {
    
    @Override
    protected SpringApplicationBuilder configure(SpringApplicationBuilder application) {
        return application.sources(MatchaiApplication.class);
    }

    public static void main(String[] args) {
        SpringApplication.run(MatchaiApplication.class, args);
    }
}
