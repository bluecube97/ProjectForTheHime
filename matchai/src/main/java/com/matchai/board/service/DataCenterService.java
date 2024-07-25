package com.matchai.board.service;

import java.util.List;
import java.util.Map;

public interface DataCenterService {
    List<Map<String, Object>> getKboBatList();

    List<Map<String, Object>> getKboPitList();

    List<Map<String, Object>> getMlbBatList();

    List<Map<String, Object>> getMlbPitList();

    List<Map<String, Object>> sortList(List<Map<String, Object>> list, String sortBy, String sortType);
}
