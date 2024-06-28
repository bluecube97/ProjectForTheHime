package com.matchai.board.config;

import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.stereotype.Component;

@Component
@ConfigurationProperties(prefix = "schedule")
public class ScheduleUtils {

    private String cron;

    // 게터 세터 생성. 혹은 lombok사용
    public String getCron() {
        return cron;
    }

    public void setCron(String cron) {
        this.cron = cron;
    }
}
