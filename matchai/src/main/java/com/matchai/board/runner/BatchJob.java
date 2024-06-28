package com.matchai.board.runner;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Component;

import com.game.baseball.api.config.BatchConfig;

@Component
public class BatchJob {

	@Autowired
	private BatchConfig batchConfig;

	// 배치 실행 시간 설정은 application.properties
	@Scheduled(cron = "${schedule.cron}")
	public void runBatch() {
		batchConfig.perform();
	}
}
