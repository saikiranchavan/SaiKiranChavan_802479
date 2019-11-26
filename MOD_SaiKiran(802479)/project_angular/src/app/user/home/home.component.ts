import { Component, OnInit } from '@angular/core';
import { Time } from '@angular/common';
import { stringify } from '@angular/compiler/src/util';
import { UserregisterService } from '../userregister.service';
import { CommonService } from 'src/app/common.service';
import { RegisterService } from 'src/app/mentor/mentor/register.service';
import { Training } from '../Training';
import { LocalStorageService } from 'ngx-webstorage';
import { TechnologyClass } from '../TechnologyClass';
import {Router} from '@angular/router';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  //mentorSearchResult
  constructor(private userService:UserregisterService,private commonService:CommonService,private mentorService:RegisterService,private localSto:LocalStorageService,private router:Router) {
    this.training=new Training();
    // this.Technologies=new TechnologyClass();
  }
  Technologies:TechnologyClass[];
  training:Training;
  a:any[];
  search:string;
  init=1;
  date1:Date;
  date2:Date;
  start_time_string:string;
  end_time_string:number;
  start_time:number;
  end_time:number;
  start_time_hour:Number;
  end_time_hour:Number;
  start_time_min:Number;
  end_time_min:Number;
  mentorSearchResult:any=[]
  object;
  fees;
  rating_array=[];
  userdata;
  proposed :boolean[];
  Acknowledge;
  check(c){
    console.log("iam here to see whether ur logged in");
    if(!this.commonService.checkLocalToken()){
      this.router.navigateByUrl('/login');
    }
    else{
      var a=this.localSto.retrieve("user");
      console.log(a["userID"]);
      this.training.uID=a["userID"];
      
      this.training.mentorID=(this.mentorSearchResult[c]).mentorID;
      this.proposed[c]=true;
      this.userdata=this.commonService.getUser();
      console.log("this is the training object "+this.training)
      this.userService.proposeMentor(this.training);
    }
  }
  
  
  
  submit(){
    var obj=this.training.technologyName;
    this.Technologies.forEach(element => {
      if((element["technologyName"])==this.training.technologyName){
        this.fees=element["fees"];
      }
    });
    this.a=this.start_time_string.split("TO");
    this.start_time=+this.a[0];
    this.end_time=+this.a[1];
    this.training.startTime=this.start_time;
    this.training.endTime=this.end_time;
    
    // var b=this.localSto.retrieve["user"]
    // this.training.userID=b["userID"];
    this.training.startTime=this.start_time;
    this.training.endTime=this.end_time;
    this.userService.SearchMentor(this.training.technologyName,this.start_time,this.end_time).subscribe(
      data=>{
        if(data==null||data==0){
          this.Acknowledge="There is No Trainer available either in this Technology or this specific Time Slot";
        }
        else{
          console.log(data);
          var i=0;
        
          this.mentorSearchResult=data;
          
          this.proposed=new Array(this.mentorSearchResult.length);
        
          for(i;i<this.mentorSearchResult.length;i++){
            this.proposed[i]=false;
          }
          
          if(this.mentorSearchResult.rating>0&&this.mentorSearchResult.rating<=5){
            this.rating_array=Array(this.mentorSearchResult.rating).fill(3);
          }
          else{
            this.rating_array=[];
          }
        }
      },
      err=>{
        //console.log("no search data is found");
        this.Acknowledge="There is No Trainer available either in this Technology or this specific Time Slot"
      });
  }
  ngOnInit() {
      this.userService.GetTechnologies().subscribe(data=>{
        
        this.Technologies=data;
        console.log(this.Technologies);
      },
      err=>{
        //this.Acknowledge="There is No Trainer available either in this Technology or this specific Time Slot"
      });
  }

}
