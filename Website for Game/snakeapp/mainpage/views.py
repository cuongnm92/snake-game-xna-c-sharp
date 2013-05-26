# Create your views here.

from django.http import HttpResponseRedirect
from django.shortcuts import render_to_response
from django.template.context import RequestContext


def main_page(request):
    return render_to_response('mainpage/home.html')