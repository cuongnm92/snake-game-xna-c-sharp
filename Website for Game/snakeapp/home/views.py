# Create your views here.

from django.http import HttpResponseRedirect
from django.shortcuts import render_to_response
from django.template.context import RequestContext

def home(request):
    return render_to_response('home/index.html')