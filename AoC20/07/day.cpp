#include <iostream>
#include <map>
#include <vector>
#include <algorithm>
#include <fstream>
#include <regex>

using namespace std;

typedef map<string, vector< pair<string,int> > > Rules;

bool canContain( const Rules & rules, const string & out, const string & in ) {
    for ( const auto & p : rules.at(out) ) {
        if ( ( p.first == in ) || canContain(rules, p.first, in ) ) return true;
    }
    return false;
}

int countIn( const Rules & rules, const string & out ) {
    int result = 0;
    for ( const auto & p : rules.at(out) ) {
        result += p.second * ( 1 + countIn(rules, p.first) );
    }
    return result;
}

int main() {
    ifstream f("input.txt");
    // ifstream f("test.txt");

    string line;

    // drab tan bags contain 4 clear gold bags.
    // vibrant lime bags contain 3 faded gold bags, 3 plaid aqua bags, 2 clear black bags.
    const regex reLine("^([\\w ]+) bags contain ([^.]+).$");
    const regex reList("([\\d]+) ([\\w ]+) bag");
    Rules rules;

    while (getline(f, line)) {
        smatch what;
        if( regex_match( line, what, reLine)) {
            const string container = what[1];
            const string all = what[2];

            vector< pair<string,int> > list;

            string::const_iterator searchStart( all.cbegin() );
            while ( regex_search( searchStart, all.cend(), what, reList ) )
            {
                list.emplace_back( what[2], stoi(what[1]) );
                searchStart = what.suffix().first;
            }
            rules.insert( make_pair( container, list ) );
        } else {
            cerr << "Unexpected line: " << line << endl;
        }
    }

    auto count = 0;
    const string color = "shiny gold";
    for ( auto & p : rules ) {
        count += canContain( rules, p.first, color);
    }
    cout << "Answer 1: " << count << endl;
    cout << "Answer 2: " << countIn(rules, color) << endl;

    return 0;
}