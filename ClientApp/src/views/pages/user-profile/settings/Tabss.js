// ** MUI Imports
import Grid from '@mui/material/Grid'

// ** Custom Components Imports
import CardSnippet from 'src/@core/components/card-snippet'

// import TabsVertical from 'src/views/components/tabs/TabsVertical'
import TabsVertical from './TabsVertical'

// ** Source code imports
import * as source from 'src/views/components/tabs/TabsSourceCode'

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