Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions


Public Class Form1
    Dim captcacode As String
    Dim othercode As String


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim request As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create("https://services.gst.gov.in/services/captcha")
        request.UserAgent = "Mozilla/5.0 (X11; Fedora; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.135 Safari/537.36"
        request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9"
        request.Connection = ConnectionState.Open
        request.Headers("Pragma") = "no-cache"
        request.Headers("Cache-Control") = "no-cache"
        request.Headers("Upgrade-Insecure-Requests") = "1"
        request.Headers("Sec-Fetch-Site") = "none"
        request.Headers("Sec-Fetch-Mode") = "navigate"
        request.Headers("Sec-Fetch-User") = "?1"
        request.Headers("Sec-Fetch-Dest") = "document"
        request.Referer = "https://services.gst.gov.in/services/searchtp"
        request.Headers("Accept-Language") = "en-IN,en-US;q=0.9,en;q=0.8"

        Dim response1 As System.Net.WebResponse = request.GetResponse()
        'MsgBox(response1.Headers.Get("set-cookie").ToString)
        Dim split As String() = Regex.Split(response1.Headers.Get("set-cookie").ToString, ";")
        captcacode = split(0).ToString & ";"
        othercode = Regex.Split(split(5).ToString, ",")(1) & ";"
        'MsgBox(othercode)
        Dim imagepic As Image
        Using stream As System.IO.Stream = response1.GetResponseStream
            imagepic = New Bitmap(System.Drawing.Image.FromStream(stream))
        End Using
        PictureBox1.Image = imagepic




    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim sreq As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create("https://services.gst.gov.in/services/api/search/taxpayerDetails")
        sreq.UserAgent = "Mozilla/5.0 (X11; Fedora; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.135 Safari/537.36"
        sreq.Accept = "application/json, text/plain, */*"
        sreq.Connection = ConnectionState.Open
        sreq.Headers("Pragma") = "no-cache"
        sreq.Headers("Cache-Control") = "no-cache"
        'sreq.Headers("Content-Type") = "application/json;charset=UTF-8"
        sreq.Headers("Origin") = "https://services.gst.gov.in"
        sreq.Headers("Sec-Fetch-Site") = "same-origin"
        sreq.Headers("Sec-Fetch-Mode") = "cors"
        sreq.Headers("Sec-Fetch-Dest") = "empty"
        sreq.Referer = "https://services.gst.gov.in/services/searchtp"
        sreq.Headers("Accept-Language") = "en-IN,en-US;q=0.9,en;q=0.8"
        sreq.Headers("Cookie") = "Lang=en; " & captcacode & othercode
        Dim enc As UTF8Encoding
        Dim postdata As String
        Dim postdatabytes As Byte()
        enc = New System.Text.UTF8Encoding()


        postdata = "{""gstin"":""27BDCPM5192H1ZD"",""captcha"":""" & TextBox1.Text.ToString & """}"
        'MsgBox(postdata)
        postdatabytes = enc.GetBytes(postdata)
        sreq.Method = "POST"
        sreq.ContentType = "application/json;charset=UTF-8;"
        sreq.ContentLength = postdatabytes.Length
        Using stream = sreq.GetRequestStream()
            stream.Write(postdatabytes, 0, postdatabytes.Length)
        End Using
        'MsgBox(sreq.Headers.ToString)

        Dim result = sreq.GetResponse()
        Dim html
        Using reader As New StreamReader(result.GetResponseStream())
            html = reader.ReadToEnd()
        End Using

        MsgBox(html.ToString)
    End Sub


End Class
