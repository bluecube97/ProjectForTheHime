package com.matchai.board.service;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.matchai.board.config.ApiDbUtils;
import com.matchai.board.config.PathUtils;
import com.matchai.board.config.ScheduleUtils;

@Service
public class ApiCombinedService {

	// db, schedule, batch세개를 한번에 관리하기 위한 sevice클래스
    private final PathUtils pathUtils;
    private final ApiDbUtils apiDbUtils;
    //private final ScheduleUtils scheduleUtils;

    @Autowired
    public ApiCombinedService(PathUtils pathUtils, ApiDbUtils apiDbUtils, ScheduleUtils scheduleUtils) {
        this.pathUtils = pathUtils;
        this.apiDbUtils = apiDbUtils;
        //this.scheduleUtils = scheduleUtils;
    }

    public String getDbUrl() {
        return apiDbUtils.getUrl();
    }

    public String getFilePath() {
        return pathUtils.getPath();
    }

    public String getMlbSchedulePyPath() {
        return pathUtils.getMlbSchedulePyPath();
    }

    public String getKboSchedulePyPath() {
        return pathUtils.getKboSchedulePyPath();
    }

    public String getMlbResultPyPath() {
        return pathUtils.getMlbResultPyPath();
    }

    public String getKboResultPyPath() {
        return pathUtils.getKboResultPyPath();
    }
}
