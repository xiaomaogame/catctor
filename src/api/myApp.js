import request from '@/utils/request'

export function GetAniInfoPost(data) {
    return request({
        url: '/api/character/GetAniInfo',
        method: 'post',
        data
    })
}

export function GetImgDataPost(data) {
    return request({
        url: '/api/character/GetImgData',
        method: 'post',
        data
    })
}

export function GetImgTablesPost(data) {
    return request({
        url: '/api/character/GetImgTables',
        method: 'post',
        data
    })
}

export function EditIconPost(data) {
    return request({
        url: '/api/character/EditIcon',
        method: 'post',
        data
    })
}

export function DelImgJsonPost(data) {
    return request({
        url: '/api/character/DelImgJson',
        method: 'post',
        data
    })
}

export function GetNamePost(data) {
    return request({
        url: '/api/character/GetName',
        method: 'post',
        data
    })
}

