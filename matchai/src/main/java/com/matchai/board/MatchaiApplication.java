package com.matchai.board;

import org.mybatis.spring.annotation.MapperScan;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.context.properties.EnableConfigurationProperties;
import org.springframework.context.annotation.ComponentScan;
import org.springframework.scheduling.annotation.EnableScheduling;

import com.matchai.board.config.ApiDbUtils;
import com.matchai.board.config.PathUtils;
import com.matchai.board.config.ScheduleUtils;

@SpringBootApplication
@MapperScan({"com.matchai.board.mapper", "com.game.baseball.api.mapper", "com.unity.game.mapper"})
@ComponentScan({"com.matchai.board", "com.game.baseball.api", "com.unity.game"})
@EnableScheduling // 스케줄링 활성화
@EnableConfigurationProperties({PathUtils.class, ApiDbUtils.class, ScheduleUtils.class})
public class MatchaiApplication {
    public static void main(String[] args) {
        SpringApplication.run(MatchaiApplication.class, args);
    }
}
