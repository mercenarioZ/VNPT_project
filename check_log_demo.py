import time
import telnetlib
import mysql.connector
from netmiko import ConnectHandler
import requests
#import cx_Oracle
import sys,os

def tra_cuu_adsl_port(user):
    mydb = mysql.connector.connect(
    host="192.168.150.206",
    user="admin",
    password="admin#mysql",
    database="phuc_DB"
    )

    # Tạo con trỏ để thao tác với cơ sở dữ liệu
    mycursor = mydb.cursor()
    sql = "SELECT * FROM `PCT_GIAMSATCHUDONG_GPON` WHERE MA_TB = %s"
    #mycursor.execute(sql, (ngay_bat_dau, ngay_ket_thuc, name_host, port))
    mycursor.execute(sql, (user,))
    rows = mycursor.fetchall()
    # for row in rows:
    #     row = row
    return rows[-1]

def tra_log(user,name_host,ngay_bat_dau,ngay_ket_thuc,slot,pon,ont):
    f = open(r'log_{}_{}_{}.txt'.format(user,ngay_bat_dau,ngay_ket_thuc),'w',encoding="utf-8")
    mydb = mysql.connector.connect(
    host="192.168.150.206",
    user="admin",
    password="admin#mysql",
    database="phuc_DB"
    )

    # Tạo con trỏ để thao tác với cơ sở dữ liệu
    mycursor = mydb.cursor()
    if name_host[8:9] == 'A':
        port = f'1/1/{slot}/{pon}/{ont}'
        
    elif name_host[8:9] == 'H':
        port = f'0-{slot}-{pon}-{ont}'

    sql = "SELECT * FROM `ALARM_LOG_DOWN_ONT` WHERE NGAY >= %s AND NGAY <= %s AND SYSTEM_ID = %s AND PORT = %s"
    mycursor.execute(sql, (ngay_bat_dau, ngay_ket_thuc, name_host, port))
    rows = mycursor.fetchall()
    array = []
    result = ''
    for row in rows:
        a = f'{row[5]},{row[0]},{row[1]},{row[2]},{row[3]},{row[4]}'
        array.append(a)

    for log in array:
        result = result + f'{log}\n'


    # result = result + f'\n\n\n****CHÚ THÍCH****\nReceived Dying Gasp: ONT mất nguồn/khách hàng tắt modem\nSignal Degraded: lỗi bit error chiều up của ONT\nRemote Defect Indication from ONT: lỗi bit error chiều down của ONT\nONT fails to respond to OMCI message requests/ONT VEIP Mismatch. ONT or LT did not accept the configuration requests for the VEIP: lỗi ONT không giao tiếp được với OLT/lỗi không đồng bộ cấu hình với OLT\nfiber is broken: ONT mất tín hiệu/lỗi quang\nONT is inactive: ONT mất tín hiệu/lỗi quang/ONT lỗi/ONT bị reboot\nloss of GEM/loss of frame: lỗi mất gói trên đường truyền\ndying-gasp: ONT mất nguồn/khách hàng tắt modem'
    # print(result)
    f.write(result)
    f.close()
    return result

def main_check(user,ngay_bat_dau,ngay_ket_thuc):

    #user = sys.argv[1]
    #ngay_bat_dau = sys.argv[2]
    #ngay_ket_thuc = sys.argv[3]

    #user = 'ctynegen'
    info = tra_cuu_adsl_port(user)
    name_host = info[0]
    if name_host[8:9] == 'A':
        adsl = info[1]
        adsl = adsl.split('/')
        slot = adsl[2]
        pon = adsl[3]
        ont = adsl[4]
        result = tra_log(user,name_host,ngay_bat_dau,ngay_ket_thuc,slot,pon,ont)

    elif name_host[8:9] == 'H':
        adsl = info[1]
        adsl = adsl.split('-')
        slot = adsl[1]
        pon = adsl[2]
        ont = adsl[3]
        result = tra_log(user,name_host,ngay_bat_dau,ngay_ket_thuc,slot,pon,ont)
    print(result)

user = sys.argv[1]
ngay_bat_dau = sys.argv[2]
ngay_ket_thuc = sys.argv[3]
# user = 'thunti5038210'
# ngay_bat_dau = '2023-08-09'
# ngay_ket_thuc = '2023-08-10'
main_check(user,ngay_bat_dau,ngay_ket_thuc)