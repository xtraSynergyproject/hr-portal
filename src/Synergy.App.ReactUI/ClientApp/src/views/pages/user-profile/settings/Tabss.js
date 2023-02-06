// ** MUI Imports
import Grid from '@mui/material/Grid'
import Typography from '@mui/material/Typography'

// ** Custom Components Imports
import CardSnippet from 'src/@core/components/card-snippet'

// ** Demo Components Imports
import TabsNav from 'src/views/components/tabs/TabsNav'
import TabsIcon from 'src/views/components/tabs/TabsIcon'
import TabsColor from 'src/views/components/tabs/TabsColor'
import TabsSimple from 'src/views/components/tabs/TabsSimple'
import TabsCentered from 'src/views/components/tabs/TabsCentered'
// import TabsVertical from 'src/views/components/tabs/TabsVertical'
import TabsVertical from './TabsVertical'
import TabsFullWidth from 'src/views/components/tabs/TabsFullWidth'
import TabsCustomized from 'src/views/components/tabs/TabsCustomized'
import TabsForcedScroll from 'src/views/components/tabs/TabsForcedScroll'
import TabsCustomizedVertical from 'src/views/components/tabs/TabsCustomizedVertical'

// ** Source code imports
import * as source from 'src/views/components/tabs/TabsSourceCode'
import { Box } from '@mui/system'

const Tabs = () => {
  return (
    <Grid container spacing={12} className='match-height' >
      <Grid item xs={10} >
        <CardSnippet
          title=''
          code={{
            tsx: null,
            jsx: source.TabsVerticalJSXCode
          }}
        >
          
          <Grid item xs={12} sx={{display:"flex",justifyContent:'center'}}>
            <Grid item xs={3} variant="h3"  component="h3">Your details</Grid> 
            <Grid item xs={9}  variant="h3"  component="h3">Public profiles.</Grid>
            </Grid>
          <TabsVertical />
        </CardSnippet>
      </Grid>
    </Grid>
  )
}

export default Tabs