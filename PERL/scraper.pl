 #
 # ----- Webscraper voor Carcassonne-database  -----
 #
 # --> vult applicationusers aan met random gebruikers
 # --> Random gebruikers worden aangemaakt op basis van de FIS-database
 #

 use LWP::UserAgent;
 use DBI;

 print "------------------------------------------------\n";
 print "----- Webscraper voor Carcassonne-database -----\n";
 print "------------------------------------------------\n\n";

 print "Initialiseren van scraper...\n";
 my $ua = LWP::UserAgent->new;
 $ua->agent("Carcassonne_scraper/0.1 ");

 my $link = "http://data.fis-ski.com/cross-country/biographies.html";
 $link .= "?listid=&lastname=&gender=ALL&sector=&firstname=&nation=&status=ALL&fiscode=&Search=Search&limit=100";
 my $req = HTTP::Request->new(GET => $link);
 $link.="&bt=next&rec_start=";
 my $current = 136400;
 my $MAX_NR_OF_TUPELS = 1000000;

 print "Starten met scrapen? (y/n)\n";

 chomp(my $ok = <>);
 my $yes = 'y';
 if ($ok eq $yes) {

  my $res = $ua->request($req);
  while ($res->is_success){# && $current < $MAX_NR_OF_TUPELS) {
  my %spelers;
  my @matches = 
  $res->content =~ /<td .*><a .*>(.*?)<\/a>.*<\/td>\n<td .*>(.*?)&nbsp;<\/td>\n<td .*>(.*?)&nbsp;<\/td>\n<td .*>(.*?)&nbsp;<\/td>/gi;

  my $i = 0;
  my $v = "";
    #volgorde van elke match is: key: naam, [0] geboortedatum, [1] sector, [2] nationaliteit
    for (@matches) {
      if($i == 0){ $v = $_; $spelers{$_}=(); }else{ $spelers{$v}[$i-1]=$_; }
      $i+=1;
      if($i==4){ $i=0; }
    }
    $| = 1;

    print "index is: ",$current," aantal sleutels: ",scalar keys %spelers,"\n";

    #$req = HTTP::Request->new(GET => $pursuelink.$current);
    $req->uri($link.$current);

    $res = $ua->request($req);
    $current+=100;
    print $res->status_line, "\n";

  #misschien evt naar file schrijven als resultset te groot zou worden, en dan uit file lezen ipv rechtstreeks uit hash
  #evt kunnen we ook lijnen rechtstreeks injecteren na elke request
  #misschien een metriek insteken om te kijken wat het beste zou zijn
  #
  open (MYFILE, '>>names.txt'); 
  print MYFILE join "\n",map{"$_ ,$spelers{$_}[0],$spelers{$_}[1],$spelers{$_}[2]"} keys %spelers; 
  print MYFILE "\n";
  close (MYFILE);

  if ((scalar keys %spelers)!=0) {
  #init db-conn
  my $dbh = DBI->connect("DBI:mysql:database=test;mysql_socket=/Applications/MAMP/tmp/mysql/mysql.sock","root", "root", {'RaiseError' => 1});

  my @chars = ("A".."Z", "a".."z",0..10);

  my $inserts = 0;

  foreach my $x (keys %spelers) { 

    foreach my $y (keys %spelers) {

      @name = (grep /(^[A-Z]*$)/,split" ",$x);
      if(@name){
        $inserts++;
        my @nameparts = grep /([a-z])/,split" ",$y;
        push(@nameparts,@name);
        my $un = lc(join "", @nameparts);
        #mss ook ander domein in email-adres dan gewoon het omgekeerde van de username?
        my $email = join "",$un,"@",(join"",@name),".",$spelers{$y}[2];
        my @date = split(/"-"/,$spelers{$y}[0]);
        my $newdate = join "-",reverse(@date); 

        my $id;
        $id .= $chars[rand @chars] for 1..20;

        #print "Will add: id=".$id." date=".$newdate." username=".$un." email=".$email."\n";
        #insert in db
        $dbh->do("insert into applicationusers (Id,Birthdate,UserName, Email) values (?,?,?,?)",undef, $id, $newdate, $un,$email);
      }
    }
  }
  #close db-conn
  $dbh->disconnect();

  print $inserts,"\n";
}
}
}
