package com.matchai.board.config;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.stereotype.Component;

@Component
@ConfigurationProperties(prefix = "file")
public class PathUtils {

    @Value("${file.path}")
    private String path;
    
    @Value("${file.mlb-schedule-py-path}")
    private String mlbSchedulePyPath;
    
    @Value("${file.kbo-schedule-py-path}")
    private String kboSchedulePyPath;
    
    @Value("${file.mlb-result-py-path}")
    private String mlbResultPyPath;
    
    @Value("${file.kbo-result-py-path}")
    private String kboResultPyPath;
    
    @Value("${file.gpt-exepect-py-path")
    private String gptExepectPyPath;
    
    // 게터 세터 생성. 혹은 lombok사용
	public String getPath() {
        return path;
    }

    public void setPath(String path) {
        this.path = path;
    }

    public String getMlbSchedulePyPath() {
        return mlbSchedulePyPath;
    }

    public void setMlbSchedulePyPath(String mlbSchedulePyPath) {
        this.mlbSchedulePyPath = mlbSchedulePyPath;
    }

    public String getKboSchedulePyPath() {
        return kboSchedulePyPath;
    }

    public void setKboSchedulePyPath(String kboSchedulePyPath) {
        this.kboSchedulePyPath = kboSchedulePyPath;
    }

    public String getMlbResultPyPath() {
        return mlbResultPyPath;
    }

    public void setMlbResultPyPath(String mlbResultPyPath) {
        this.mlbResultPyPath = mlbResultPyPath;
    }

    public String getKboResultPyPath() {
        return kboResultPyPath;
    }

    public void setKboResultPyPath(String kboResultPyPath) {
        this.kboResultPyPath = kboResultPyPath;
    }
    
    public String getGptExepectPyPath() {
		return gptExepectPyPath;
	}

	public void setGptExepectPyPath(String gptExepectPyPath) {
		this.gptExepectPyPath = gptExepectPyPath;
	}
}
