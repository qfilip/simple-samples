import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

/*
    --PURPOSE--
    This component is the example of how HTML can be rendered through code.
    In here, links are used
*/

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
    constructor(private router: Router) { }

    navLinks: ILink[];
    linkText: string;
    
    ngOnInit() {
        this.generateLinks();
    }
    title = 'angular-samples';

    private generateLinks() {
        const mkLink = (text: string, icon: string, path: string) => {
            return {text: text, icon: icon, path: path } as ILink;
        }
        this.navLinks = [
            mkLink('Dynamic Rendering', '<i class="lab la-edge"></i>','dynamic-rendering'),
            mkLink('Projections', '<i class="las la-list-ul"></i>','projection-rendering'),
        ];
    }

    navigateTo(route: string) {
        const navigateTo = `/${route}`;
        this.router.navigate([navigateTo]);
    }

    displayLinkText(text: string) {
        this.linkText = text;
    }

    hideLinkText() {
        this.linkText = null;
    }
}

export interface ILink {
    path: string;
    icon: string;
    text: string;
}
