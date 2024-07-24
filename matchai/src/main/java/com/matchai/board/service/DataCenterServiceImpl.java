package com.matchai.board.service;

import com.matchai.board.dao.DataCenterDao;
import org.apache.ibatis.annotations.UpdateProvider;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.*;

@Service
public class DataCenterServiceImpl implements DataCenterService {

    @Autowired
    private DataCenterDao dataCenterDao;

    @Override
    public List<Map<String, Object>> getKboBatList() {
        return dataCenterDao.getKboBatList();
    }

    @Override
    public List<Map<String, Object>> getKboPitList() {
        return dataCenterDao.getKboPitList();
    }

    @Override
    public List<Map<String, Object>> getMlbBatList() {
        return dataCenterDao.getMlbBatList();
    }

    @Override
    public List<Map<String, Object>> getMlbPitList() {
        return dataCenterDao.getMlbPitList();
    }

    @Override
    public List<Map<String, Object>> sortList(List<Map<String, Object>> list, String sortBy, String sortType) {
        Comparator<Map<String, Object>> comparator = (o1, o2) -> {
            Comparable value1 = (Comparable) o1.get(sortBy);
            Comparable value2 = (Comparable) o2.get(sortBy);
            return sortType.equals("asc") ? value1.compareTo(value2) : value2.compareTo(value1);
        };

        if (!list.get(0).containsKey(sortBy)) return list;

        PriorityQueue<Map<String, Object>> pq = new PriorityQueue<>(comparator);
        pq.addAll(list);

        List<Map<String, Object>> sortedList = new ArrayList<>();
        while (!pq.isEmpty()) {
            sortedList.add(pq.poll());
        }

        return sortedList;
    }
}
